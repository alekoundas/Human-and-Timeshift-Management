using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
     public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.HasIndex(x => x.StartOn);
            builder.HasIndex(x => x.EndOn);
        }
    }
}
