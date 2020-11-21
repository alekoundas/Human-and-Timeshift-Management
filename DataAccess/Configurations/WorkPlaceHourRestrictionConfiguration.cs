using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkPlaceHourRestrictionConfiguration : IEntityTypeConfiguration<WorkPlaceHourRestriction>
    {
        public void Configure(EntityTypeBuilder<WorkPlaceHourRestriction> builder)
        {
            builder.HasIndex(x => new { x.WorkPlaceId, x.Month, x.Year })
                .IsUnique();

            builder.HasOne(x => x.WorkPlace)
                .WithMany(x => x.WorkPlaceHourRestrictions)
                .HasForeignKey(x => x.WorkPlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
