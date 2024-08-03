namespace KonChargeAPI.ChargingStations
{
    public class StationSelectionData
    {
        public string? outletType { get; set; }
        public double? maxCapacity { get; set; }
        /// <summary>
        /// Range 0 to 1
        /// </summary>
        public double? currentPercentage { get; set; }

        public double? maxPrice { get; set; }
        public double? maxDistance { get; set; }

        public List<PriorityItem>? priorities { get; set; }
    }

    public class PriorityItem
    {
        /// <summary>
        /// Distance, Price, Speed
        /// </summary>
        public string? priorityName { get; set; }
        /// <summary>
        /// Range 0 to 1
        /// </summary>
        public double? priority {  get; set; }
    }
}
