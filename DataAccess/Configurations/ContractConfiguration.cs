using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();

            builder.HasOne(x => x.ContractMembership)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.ContractMembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ContractType)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.ContractTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
