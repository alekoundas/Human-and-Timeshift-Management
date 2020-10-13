using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
     public class TimeShiftConfiguration : IEntityTypeConfiguration<TimeShift>
    {
        public void Configure(EntityTypeBuilder<TimeShift> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
