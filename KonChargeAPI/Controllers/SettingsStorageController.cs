using KonChargeAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KonChargeAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SettingsStorageController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsStorageController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetSetting")]
        public async Task<IActionResult> Get([FromQuery] string settingKey)
        {
            if (String.IsNullOrEmpty(settingKey))
                return NoContent();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            Dictionary<string, string> settings = user.GetSettingsDict();

            if (settings.TryGetValue(settingKey, out var val))
                return Ok("{ \"content\": \"" + val + "\"}");

            return NotFound("Setting does not exist");
        }

        [HttpPost("SetSetting")]
        public async Task<IActionResult> Post([FromQuery] string settingKey, [FromBody] string settingValue)
        {
            if (String.IsNullOrEmpty(settingKey))
                return NoContent();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            Dictionary<string, string> settings = user.GetSettingsDict();
            if (settings.ContainsKey(settingKey))
                settings[settingKey] = settingValue;
            else
                settings.Add(settingKey, settingValue);

            user.SetSettingsDict(settings);
            await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}
