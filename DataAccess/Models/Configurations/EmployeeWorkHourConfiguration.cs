using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Models.Configurations
{
    public class EmployeeWorkHourConfiguration : IEntityTypeConfiguration<EmployeeWorkHour>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkHour> builder)
        {
            builder.HasOne(u => u.Employee).WithMany(u => u.EmployeeWorkHours).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasKey(t => new { t.Id, t.EmployeeId });
            builder.HasOne(pt => pt.WorkHour).WithMany(p => p.EmployeeWorkHours).HasForeignKey(pt => pt.WorkHourId);
        }
    }
}
