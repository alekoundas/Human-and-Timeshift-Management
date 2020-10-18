using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkPlaceHourRestrictionConfiguration : IEntityTypeConfiguration<WorkPlaceHourRestriction>
    {
        public void Configure(EntityTypeBuilder<WorkPlaceHourRestriction> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
