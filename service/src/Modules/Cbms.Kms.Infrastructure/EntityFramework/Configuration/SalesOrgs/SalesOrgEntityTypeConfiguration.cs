using Cbms.Authorization.Users;
using Cbms.Kms.Domain.SalesOrgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.SalesOrgs
{
    public class SalesOrgEntityTypeConfiguration : IEntityTypeConfiguration<SalesOrg>
    {
        private readonly ModelBuilder _modelBuilder;

        public SalesOrgEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<SalesOrg> builder)
        {
            //_modelBuilder.HasSequence("brand_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("SalesOrgs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            //builder.Property(x => x.Id).UseHiLo("brand_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.ParentId).IsRequired(true);
            builder.Property(x => x.TypeId).IsRequired(true);
            builder.Property(x => x.TypeName).HasMaxLength(200).IsUnicode(true).IsRequired(true);
            builder.Property(x => x.UpdateDate).IsRequired(true);

            builder.HasIndex(x => x.ParentId);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
