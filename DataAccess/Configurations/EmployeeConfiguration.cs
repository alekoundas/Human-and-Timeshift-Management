using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasIndex(x => x.VatNumber).IsUnique();

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Contract)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.ContractId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Specialization)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.SpecializationId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
