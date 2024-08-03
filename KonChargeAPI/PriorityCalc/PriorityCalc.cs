using KonChargeAPI.ChargingStationData;
using KonChargeAPI.ChargingStations;

namespace KonChargeAPI.Calc
{
    public class PriorityCalc
    {
        private StationSelectionData filter;
        private List<StationData> data;

        public PriorityCalc(StationSelectionData filter, List<StationData> input) 
        {
            this.filter = filter;
            this.data = input;
        }

        public List<StationData> CalculatePriorities ()
        {
            PriorityItem? distanceP = null;
            PriorityItem? priceP = null;
            PriorityItem? speedP = null;

            if (filter.priorities == null || filter.priorities.Count == 0)
            {
                //Default priority: Smallest distance
                distanceP = new PriorityItem()
                {
                    priorityName = "distance",
                    priority = 1
                };
            }
            else
            {
                distanceP = filter.priorities!.FirstOrDefault(t => t.priorityName == "distance");
                priceP = filter.priorities!.FirstOrDefault(t => t.priorityName == "price");
                speedP = filter.priorities!.FirstOrDefault(t => t.priorityName == "speed");
            }

            distanceP ??= new PriorityItem()
            {
                priorityName = "distance",
                priority = 0
            };
            priceP ??= new PriorityItem()
            {
                priorityName = "price",
                priority = 0
            };
            speedP ??= new PriorityItem()
            {
                priorityName = "speed",
                priority = 0
            };

            return CalculateFromPriorities(distanceP, priceP, speedP);
        }

        private List<StationData> CalculateFromPriorities (PriorityItem distanceP, PriorityItem priceP, PriorityItem speedP)
        {
            var lookup = GetStationPriorities();

            foreach (var station in data)
            {
                StationPriorities p = lookup[station];

                station.userSettingAccuracy = 0;
                station.userSettingAccuracy += p.distanceEval * distanceP.priority ?? 0;
                station.userSettingAccuracy += p.priceEval * priceP.priority ?? 0;
                station.userSettingAccuracy += p.speedEval * speedP.priority ?? 0;
            }

            return data;
        }

        private Dictionary<StationData, StationPriorities> GetStationPriorities ()
        {
            GetDistanceMinMax(out var minDistance, out var maxDistance);
            GetPriceMinMax(out var minPrice, out var maxPrice);
            GetSpeedMinMax(out var minSpeed, out var maxSpeed);

            Dictionary<StationData, StationPriorities> lookup = new Dictionary<StationData, StationPriorities>();

            foreach (var item in data)
            {
                lookup.Add(item, new StationPriorities
                {
                    distanceEval = 1 - EvalValue(item.airDistance, minDistance, maxDistance),
                    priceEval = 1 - EvalValue(item.GetPricePerKwh(filter.outletType!), minPrice, maxPrice),
                    speedEval = EvalValue(item.GetMaxChargingSpeed(), minSpeed, maxSpeed),
                });
            }

            return lookup;
        }

        private double EvalValue (double val, double min, double max)
        {
            return (val - min) / max;
        }
        
        private void GetDistanceMinMax (out double min, out double max)
        {
            min = data.Min(t => t.airDistance);
            max = data.Max(t => t.airDistance);
        }

        private void GetSpeedMinMax (out double min, out double max)
        {
            min = data.Min(t => t.GetMaxChargingSpeed());
            max = data.Max(t => t.GetMaxChargingSpeed());
        }

        private void GetPriceMinMax (out double min, out double max)
        {
            min = data.Min(t => t.GetPricePerKwh(filter.outletType!));
            max = data.Max(t => t.GetPricePerKwh(filter.outletType!));
        }
    }
}
