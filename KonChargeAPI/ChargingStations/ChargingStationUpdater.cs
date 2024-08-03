
using Newtonsoft.Json;

namespace KonChargeAPI.ChargingStationData
{
    public class ChargingStationUpdater
    {
        /// <summary>
        /// Dataset request url for stations that filters for active and available stations
        /// </summary>
        //private const string DATASET_URL = "https://mobility.api.opendatahub.com/v2/flat/EChargingStation?limit=-1&offset=0&where=and(sactive.eq.True,savailable.eq.True)";

        private const string DATASET_URL = "https://mobility.api.opendatahub.com/v2/flat/EChargingPlug?limit=-1&offset=0&where=and(sactive.eq.True,savailable.eq.True)";

        public ChargingStationData? data;

        public async Task LoadChargingStationData ()
        {
            HttpClient client = new HttpClient();

            // GET-Anfrage an die API
            var response = await client.GetAsync(DATASET_URL);
            client.Dispose();

            if (!response.IsSuccessStatusCode)
                return;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            data = JsonConvert.DeserializeObject<ChargingStationData>(jsonResponse);
        }
    }
}
