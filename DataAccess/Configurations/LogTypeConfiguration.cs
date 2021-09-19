using DataAccess.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class LogTypeConfiguration : IEntityTypeConfiguration<LogType>
    {
        public void Configure(EntityTypeBuilder<LogType> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();

        }
    }
}
