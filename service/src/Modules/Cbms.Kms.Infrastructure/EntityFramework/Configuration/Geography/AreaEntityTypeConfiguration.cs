using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Areas
{
    internal class AreaEntityTypeConfiguration : IEntityTypeConfiguration<Area>
    {
        private readonly ModelBuilder _modelBuilder;

        public AreaEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Area> builder)
        {
            _modelBuilder.HasSequence("area_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("Areas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("area_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);
            builder.Property(x => x.SalesOrgId).HasDefaultValue(0);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
