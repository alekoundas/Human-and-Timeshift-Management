using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkHourConfiguration : IEntityTypeConfiguration<WorkHour>
    {
        public void Configure(EntityTypeBuilder<WorkHour> builder)
        {
            builder.HasIndex(x => new { x.StartOn, x.EndOn, x.EmployeeId })
                .IsUnique();

            builder.HasOne(x => x.TimeShift)
                .WithMany(x => x.WorkHours)
                .HasForeignKey(x => x.TimeShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.WorkHours)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
