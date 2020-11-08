﻿using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class HourRestrictionConfiguration : IEntityTypeConfiguration<HourRestriction>
    {
        public void Configure(EntityTypeBuilder<HourRestriction> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}