using KonChargeAPI.Calc;
using KonChargeAPI.ChargingStationData;
using KonChargeAPI.Controllers;

namespace KonChargeAPI.ChargingStations
{
    public class ChargingStationSelector
    {
        public List<StationData> data;

        public ChargingStationSelector(List<StationData> data) 
        {
            this.data = data;
        }

        public void CalculateRequiredValues (StationSelectionData filter, double duration)
        {
            foreach (var station in data)
            {
                double chargePower = station.GetMaxChargingSpeed();

                double energyAdded = chargePower * duration;

                double percentageAdded = energyAdded / filter.maxCapacity ?? 1;

                station.endBatteryLevel = Math.Clamp(filter.currentPercentage + percentageAdded ?? 0, 0, 1);
            }
        }

        public void SelectChargingStations (StationSelectionData filter)
        {
            List<Coordinate> uniqueCoords = new List<Coordinate>();

            //Hard settings that remove stations
            for (int i = data.Count - 1; i >= 0; i--)
            {
                StationData? station = data[i];

                if (uniqueCoords.Contains(station.scoordinate!))
                {
                    data.RemoveAt(i);
                    continue;
                }

                List<PlugOutlet> metadata = station.smetadata!.outlets!;

                metadata = metadata.Where(t => t.outletTypeCode == filter.outletType).ToList();

                station.smetadata.outlets = metadata;

                if (!metadata.Any())
                    data.RemoveAt(i);
                else
                {
                    if (station.airDistance > filter.maxDistance)
                        data.RemoveAt(i);
                    else if (station.GetPricePerKwh(filter.outletType!) > filter.maxPrice)
                        data.RemoveAt(i);
                    else
                        uniqueCoords.Add(station.scoordinate!);
                }
            }

            if (data.Count == 0)
                return;

            PriorityCalc calc = new PriorityCalc(filter, data);

            data = calc.CalculatePriorities();

            //Sort by distance
            data = data.OrderByDescending(t => t.userSettingAccuracy).ToList();

            data.First().type = "Best"
        }
    }
}
