using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            //builder.HasMany(x => x.Customers)
            //    .WithOne(x => x.Company)
            //    .HasForeignKey(x => x.CompanyId)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.HasMany(x => x.Employees)
            //    .WithOne(x => x.Company)
            //    .HasForeignKey(x => x.CompanyId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
