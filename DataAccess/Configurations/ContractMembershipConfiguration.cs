using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ContractMembershipConfiguration : IEntityTypeConfiguration<ContractMembership>
    {
        public void Configure(EntityTypeBuilder<ContractMembership> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
