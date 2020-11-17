using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TimeShiftConfiguration : IEntityTypeConfiguration<TimeShift>
    {
        public void Configure(EntityTypeBuilder<TimeShift> builder)
        {
            builder.HasIndex(x => new { x.WorkPlaceId, x.Month, x.Year }).IsUnique();
        }
    }
}
