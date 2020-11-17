using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EmployeeWorkPlaceConfiguration : IEntityTypeConfiguration<EmployeeWorkPlace>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkPlace> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasKey(t => new { t.Id, t.EmployeeId });


            builder.HasOne(u => u.Employee)
                .WithMany(u => u.EmployeeWorkPlaces)
                .IsRequired();

            builder.HasOne(pt => pt.WorkPlace)
                .WithMany(p => p.EmployeeWorkPlaces)
                .HasForeignKey(pt => pt.WorkPlaceId);

        }
    }
}
