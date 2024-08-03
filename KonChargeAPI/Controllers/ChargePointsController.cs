using KonChargeAPI.ChargingStationData;
using KonChargeAPI.ChargingStations;
using KonChargeAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Numerics;

namespace KonChargeAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class ChargePointsController : ControllerBase
    {
        private const double EARTH_RADIUS_KM = 6371.0;
        /// <summary>
        /// Maximum filter distance in km
        /// </summary>
        public double MAX_DISTANCE = 1;

        public ChargePointsController()
        { }

        [HttpGet("Load")]
        public async Task<IActionResult> Get([FromQuery]double lat, [FromQuery]double lng, [FromQuery]int duration, [FromQuery]string selectionConfig)
        {
            // Validate latitude
            if (lat < -90.0 || lat > 90.0)
                return BadRequest("Latitude must be between -90.0 and 90.0.");
            
            // Validate longitude
            if (lng < -180.0 || lng > 180.0)
                return BadRequest("Longitude must be between -180.0 and 180.0.");
            
            if (duration < 0)
                return BadRequest("Duration must be a positive integer.");

            StationSelectionData? filter = JsonConvert.DeserializeObject<StationSelectionData>(selectionConfig);

            if (filter == null)
                return BadRequest("Filter not valid");

            ChargingStationUpdater updater = new ChargingStationUpdater();

            await updater.LoadChargingStationData();

            if (updater.data == null || updater.data!.data == null)
                return NotFound("Data was null");

            //Search for all charging stations near user
            var result = updater.data!.data!.Where(t =>
            {
                t.airDistance = CalculateDistance(t.scoordinate!.y, t.scoordinate!.x, lat, lng);
                return t.airDistance < MAX_DISTANCE;
            }).Distinct().ToList();

            if (result.Count <= 0)
                return NotFound("No charging station found");

            //ChargingPlugUpdater plugLoader = new ChargingPlugUpdater(result);
            //await plugLoader.LoadPlugs();

            ChargingStationSelector selector = new ChargingStationSelector(result);
            selector.CalculateRequiredValues(filter, duration);
            selector.SelectChargingStations(filter);
            result = selector.data;

            ChargingStationRouter routeCalc = new ChargingStationRouter(result);
            await routeCalc.CalculateChargingStationRoutes(lng, lat);

            var dataRes = new KonChargeAPI.ChargingStationData.ChargingStationData()
            {
                data = result
            };

            string output = JsonConvert.SerializeObject(dataRes);

            return Ok(output);
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EARTH_RADIUS_KM * c;
        }

        public static double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }
    }
}
