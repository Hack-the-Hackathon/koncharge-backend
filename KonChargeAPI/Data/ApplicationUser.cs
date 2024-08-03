using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

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
        public string UserSettings { get; set; } = "";

        public Dictionary<string, string> GetSettingsDict ()
        {
            if (String.IsNullOrEmpty(UserSettings))
                return new Dictionary<string, string>();

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(UserSettings) 
                ?? new Dictionary<string, string>();
        }

        public void SetSettingsDict (Dictionary<string, string> dict)
        {
            UserSettings = JsonConvert.SerializeObject(dict);
        }
    }
}
