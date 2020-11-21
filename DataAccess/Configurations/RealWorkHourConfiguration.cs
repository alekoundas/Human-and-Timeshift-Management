using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RealWorkHourConfiguration : IEntityTypeConfiguration<RealWorkHour>
    {
        public void Configure(EntityTypeBuilder<RealWorkHour> builder)
        {
            builder.HasIndex(x => new { x.StartOn, x.EndOn, x.EmployeeId })
                .IsUnique();


            builder.HasOne(x => x.Employee)
                .WithMany(x => x.RealWorkHours)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TimeShift)
                .WithMany(x => x.RealWorkHours)
                .HasForeignKey(x => x.TimeShiftId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
