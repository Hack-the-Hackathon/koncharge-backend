using Microsoft.EntityFrameworkCore;

namespace KonChargeAPI.ChargingStationData
{
    public class ChargingStationData
    {
        public List<StationData>? data {  get; set; }
    }

    public class StationData
    {
        private const double DC_PRICE_PER_KWH = 0.89;
        private const double AC_PRICE_PER_KWH = 0.65;

        /// <summary>
        /// , Nearest, Best
        /// </summary>
        public string? type;

        /// <summary>
        /// Is this charging station currently available?
        /// </summary>
        public bool? savailable { get; set; }

        /// <summary>
        /// Station name
        /// </summary>
        public string? sname { get ; set; }

        /// <summary>
        /// Coordinates of this StationData
        /// </summary>
        public Coordinate? scoordinate { get; set; }

        /// <summary>
        /// The route that gets calculated
        /// </summary>
        public RouteData? route;

        /// <summary>
        /// Metadata
        /// </summary>
        public PlugMetaData? smetadata { get; set; }

        /// <summary>
        /// Accuracy of the route based on the user config between 0-1
        /// </summary>
        public double userSettingAccuracy;

        /// <summary>
        /// Distance between defined start point and this station
        /// </summary>
        public double airDistance;

        /// <summary>
        /// When charging with this charger on the specified duration
        /// 0-1 range
        /// </summary>
        public double endBatteryLevel;

        public double GetMaxChargingSpeed ()
        {
            return smetadata!.outlets!.Max(x => x.maxPower);
        }

        public double GetPricePerKwh (string chargerType)
        {
            return chargerType switch
            {
                "CHAdeMO" => DC_PRICE_PER_KWH,
                "CCS" => (DC_PRICE_PER_KWH + AC_PRICE_PER_KWH) / 2.0,
                "Type2Mennekes" => AC_PRICE_PER_KWH,
                _ => throw new NotImplementedException()
            };
        }
    }

    public class Coordinate : IEquatable<Coordinate>
    {
        /// <summary>
        /// Longitude
        /// </summary>
        public double x { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double y { get; set; }

        public bool Equals(Coordinate? other)
        {
            if (other == null)
                return false;

            return x == other.x && y == other.y;
        }
    }

    public class RouteData
    {
        /// <summary>
        /// The time it takes for the user to reach this charger
        /// </summary>
        public double routeTime;
        /// <summary>
        /// The distance of the route in km
        /// </summary>
        public double routeDistance;
    }

    public class PlugListData
    {
        /// <summary>
        /// Plugs of this station
        /// </summary>
        public List<PlugData>? data { get; set; }
    }

    public class PlugData
    {
        public PlugMetaData? smetadata { get; set; }
    }

    public class PlugMetaData
    {
        public List<PlugOutlet>? outlets { get; set; }
    }

    public class PlugOutlet
    {
        public double maxPower { get; set; }
        public double maxCurrent { get; set; }
        public double minCurrent { get; set; }
        public bool hasFixedCable { get; set; }
        public string? outletTypeCode { get; set; }
    }
}
