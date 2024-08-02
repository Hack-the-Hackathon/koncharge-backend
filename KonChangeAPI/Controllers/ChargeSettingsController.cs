using KonChargeAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KonChargeAPI.Controllers
{
    /// <summary>
    /// Simple controller to get and set the charge settings, has a get and post endpoint
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ChargeSettingsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChargeSettingsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetChargeSettings")]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user.UserChargeSettings);
        }

        [HttpPost("SetChargeSettings")]
        public async Task<IActionResult> Post([FromBody] string newChargeSettings)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            user.UserChargeSettings = newChargeSettings;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok("Charge settings updated successfully.");
            else
                return BadRequest("Failed to update charge settings.");
        }
    }
}
