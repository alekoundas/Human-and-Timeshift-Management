
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
     public class WorkPlaceConfiguration : IEntityTypeConfiguration<WorkPlace>
    {
        public void Configure(EntityTypeBuilder<WorkPlace> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
