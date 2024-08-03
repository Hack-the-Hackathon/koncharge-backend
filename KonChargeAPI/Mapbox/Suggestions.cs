namespace KonChargeAPI.Mapbox
{
    public class Suggestions
    {
        public List<SuggestionData>? suggestions {  get; set; }
    }

    public class SuggestionData
    {
        public string? name { get; set; }
        public string? mapbox_id { get; set; }
    }
}
