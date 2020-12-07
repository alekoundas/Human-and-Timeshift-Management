using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole,
        ApplicationUserLogin, IdentityRoleClaim<string>, ApplicationUserToken>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
        : base(options)
        {
        }
        public DbSet<ApplicationTag> ApplicationTags { get; set; }
        public DbSet<ApplicationUserTag> ApplicationUserTags { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }


    }
}
