using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KonChargeAPI.Data
{
    /// <summary>
    /// Contains the identity data for the auth
    /// </summary>
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions options) : base(options) { }
    }
}
