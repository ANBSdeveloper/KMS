using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.SalesOrgs;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;


namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Staffs
{
    internal class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        private readonly ModelBuilder _modelBuilder;

        public StaffEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            _modelBuilder.HasSequence("staff_seq").IncrementsBy(100).StartsAt(1);

            builder.ToTable("Staffs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("staff_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.UpdateDate).IsRequired(true);
            builder.Property(x => x.AreaId).IsRequired(false);
            builder.Property(x => x.ZoneId).IsRequired(false);

            builder.Property(x => x.StaffTypeCode).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.StaffTypeName).IsRequired(true).HasMaxLength(200).IsUnicode(true);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.MobilePhone).HasMaxLength(20);
            builder.Property(x => x.Birthday).IsRequired(false);

            builder.HasOne<Area>().WithMany().HasForeignKey(p => p.AreaId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<SalesOrg>().WithMany().HasForeignKey(p => p.SalesOrgId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
