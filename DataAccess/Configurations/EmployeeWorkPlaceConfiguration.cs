using System;
using System.Collections.Generic;
using System.Text;
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


            builder.HasOne(u => u.Employee).WithMany(u => u.EmployeeWorkPlaces).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasKey(t => new { t.Id, t.EmployeeId });
            builder.HasOne(pt => pt.WorkPlace).WithMany(p => p.EmployeeWorkPlaces).HasForeignKey(pt => pt.WorkPlaceId);

            builder.Property(i => i.Id).ValueGeneratedOnAdd();
        }
    }
}
