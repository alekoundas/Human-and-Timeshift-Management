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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUserRole>()
        //        .HasNoKey();

        //    modelBuilder.Entity<ApplicationUserLogin>()
        //        .HasNoKey();

        //    modelBuilder.Entity<ApplicationUserToken>()
        //        .HasNoKey();


        //    modelBuilder.Entity<ApplicationUserTag>()
        //        .HasKey(bc => new { bc.ApplicationUserId, bc.ApplicationTagId });

        //    modelBuilder.Entity<ApplicationUserTag>()
        //        .HasOne(bc => bc.ApplicationUser)
        //        .WithMany(b => b.ApplicationUserTags)
        //        .HasForeignKey(bc => bc.ApplicationUserId);

        //    modelBuilder.Entity<ApplicationUserTag>()
        //        .HasOne(bc => bc.ApplicationTag)
        //        .WithMany(c => c.ApplicationUserTags)
        //        .HasForeignKey(bc => bc.ApplicationTagId);
        //}

    }
}
