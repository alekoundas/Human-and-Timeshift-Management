using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkPlaceConfiguration : IEntityTypeConfiguration<WorkPlace>
    {
        public void Configure(EntityTypeBuilder<WorkPlace> builder)
        {
            builder.HasIndex(x => new { x.Title, x.CustomerId }).IsUnique();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.WorkPlaces)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
