using DataAccess.Configurations;
using DataAccess.Models.Security;
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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEntity> LogEntities { get; set; }
        public DbSet<LogType> LogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LogConfiguration());
            builder.ApplyConfiguration(new LogTypeConfiguration());
            builder.ApplyConfiguration(new LogEntityConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());



            //Application user roles
            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}
