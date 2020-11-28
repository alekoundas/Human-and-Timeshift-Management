using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasIndex(x => x.VatNumber).IsUnique();

            builder.HasOne(x => x.Company)
               .WithMany(x => x.Customers)
               .HasForeignKey(x => x.CompanyId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
