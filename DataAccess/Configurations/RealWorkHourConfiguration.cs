using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RealWorkHourConfiguration : IEntityTypeConfiguration<RealWorkHour>
    {
        public void Configure(EntityTypeBuilder<RealWorkHour> builder)
        {
            builder.HasIndex(x => new { x.StartOn, x.EndOn, x.EmployeeId }).IsUnique();
        }
    }
}
