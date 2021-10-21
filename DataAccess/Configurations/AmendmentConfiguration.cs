using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AmendmentConfiguration : IEntityTypeConfiguration<Amendment>
    {
        public void Configure(EntityTypeBuilder<Amendment> builder)
        {
            builder.HasIndex(x => x.Id);

            builder.HasOne(x => x.RealWorkHour)
                .WithMany();


            builder.HasOne(x => x.TimeShift)
                .WithMany(x => x.Amendments)
                .HasForeignKey(x => x.TimeShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.Amendments)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
