using Microsoft.AspNetCore.Identity;

namespace KonChargeAPI.Data
{
    /// <summary>
    /// The user object, derives from IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The settings for the user to filter for chargers
        /// </summary>
        public string UserChargeSettings { get; set; } = "{}";
    }
}
