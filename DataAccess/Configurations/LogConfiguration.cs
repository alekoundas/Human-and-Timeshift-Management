using DataAccess.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder
                .HasIndex(x => new { x.Id })
                .IsUnique();

            builder.HasOne(x => x.LogEntity)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.LogEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LogType)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.LogTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.ApplicationUserId);
        }
    }
}
