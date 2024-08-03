using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using Newtonsoft.Json.Linq;
using KonChargeAPI.Mapbox;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace KonChargeAPI.Controllers
{
    /// <summary>
    /// Integrates mapbox for autocompletion and getting coordinates
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class MapboxController : ControllerBase
    {
        public MapboxController() { }

        [HttpGet("GetAutocompletion")]
        public async Task<IActionResult> GetAutocomplete([FromQuery] string text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                    return BadRequest("Text is empty");

                HttpClient client = new HttpClient();

                string sessionToken = Guid.NewGuid().ToString();
                string url = $"https://api.mapbox.com/search/searchbox/v1/suggest?q={Uri.EscapeDataString(text)}&session_token={sessionToken}&language=en&limit=3&country=IT&access_token={SecretKeys.MAPBOX_API}";

                var response = await client.GetAsync(url);
                client.Dispose();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                Suggestions? suggestions = JsonConvert.DeserializeObject<Suggestions>(jsonResponse);

                if (suggestions == null)
                    return BadRequest("No suggestions found");

                return Ok(JsonConvert.SerializeObject(suggestions));
            }
            catch (Exception)
            {
                return BadRequest("An error occured");
            }
        }

        [HttpGet("GetCoordinates")]
        public async Task<IActionResult> GetCoordinates([FromQuery] string mapbox_id)
        {
            try
            {
                if (String.IsNullOrEmpty(mapbox_id))
                    return BadRequest("Mapbox id is empty");

                HttpClient client = new HttpClient();

                string sessionToken = Guid.NewGuid().ToString();
                string url = $"https://api.mapbox.com/search/searchbox/v1/retrieve/" + mapbox_id + "?access_token=" + SecretKeys.MAPBOX_API + "&session_token=" + sessionToken;

                var response = await client.GetAsync(url);
                client.Dispose();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(jsonResponse);

                string? coordinates = jsonObject?["features"]?[0]?["properties"]?["coordinates"]?.ToString();

                return Ok(coordinates);
            }
            catch (Exception)
            {
                return BadRequest("An error occured");
            }
        }
    }
}
