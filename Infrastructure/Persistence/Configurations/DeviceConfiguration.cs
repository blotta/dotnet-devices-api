using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(d => d.State)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.CreatedAt)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasIndex(d => d.Brand);
            builder.HasIndex(d => d.State);
        }
    }
}
