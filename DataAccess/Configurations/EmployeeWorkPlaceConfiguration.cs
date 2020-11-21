using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EmployeeWorkPlaceConfiguration : IEntityTypeConfiguration<EmployeeWorkPlace>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkPlace> builder)
        {
            builder.HasIndex(t => new { t.WorkPlaceId, t.EmployeeId }).IsUnique();

            builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

            builder.HasOne(u => u.Employee)
                .WithMany(u => u.EmployeeWorkPlaces)
                .HasForeignKey(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(pt => pt.WorkPlace)
                .WithMany(p => p.EmployeeWorkPlaces)
                .HasForeignKey(pt => pt.WorkPlaceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
