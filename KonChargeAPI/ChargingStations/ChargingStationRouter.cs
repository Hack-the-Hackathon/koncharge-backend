using Newtonsoft.Json.Linq;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;

namespace KonChargeAPI.ChargingStationData
{
    public class ChargingStationRouter
    {
        private const string DIRECTION_URL = "https://api.mapbox.com/directions/v5/mapbox/driving/";

        private List<StationData> data;

        public ChargingStationRouter(List<StationData> data) 
        { 
            this.data = data;
        }

        public async Task CalculateChargingStationRoutes (double startLng, double startLat)
        {
            HttpClient client = new HttpClient();

            string startCoords = $"{startLng},{startLat};";

            StationData? topStation = data.OrderByDescending(t => t.userSettingAccuracy).FirstOrDefault();
            if (topStation == null)
                return;

            string requestURL = DIRECTION_URL + startCoords + $"{topStation.scoordinate!.x},{topStation.scoordinate!.y}?alternatives=false&annotations=distance,duration&geometries=geojson&language=en&overview=full&steps=true&access_token={SecretKeys.MAPBOX_API}";
            var response = await client.GetAsync(requestURL);
            client.Dispose();

            if (!response.IsSuccessStatusCode)
                return;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(jsonResponse);

            var distance = jsonObject?["routes"]?[0]?["distance"]?.Value<double>();
            var duration = jsonObject?["routes"]?[0]?["duration"]?.Value<double>();

            if (distance == null || duration == null)
                return;

            topStation.route = new RouteData()
            {
                routeDistance = (distance ?? 0) / 1000,
                routeTime = duration ?? 0,
            };
        }
    }
}
