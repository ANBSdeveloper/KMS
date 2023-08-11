using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.MaterialTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Materials
{
    internal class MaterialEntityTypeConfiguration : IEntityTypeConfiguration<Material>
    {
        private readonly ModelBuilder _modelBuilder;

        public MaterialEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Material> builder)
        {
            _modelBuilder.HasSequence("material_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Materials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("material_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Description).HasMaxLength(2000).IsUnicode(true);
            builder.Property(x => x.Value).HasColumnType("decimal(18,2)");

            builder.HasOne<MaterialType>().WithMany().HasForeignKey(p => p.MaterialTypeId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
