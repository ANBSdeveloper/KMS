using Cbms.Authorization.Users;
using Cbms.Kms.Domain.InvestmentSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Infrastructure
{
    internal class InvestmentSettingEntityTypeConfiguration : IEntityTypeConfiguration<InvestmentSetting>
    {
        private readonly ModelBuilder _modelBuilder;

        public InvestmentSettingEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<InvestmentSetting> builder)
        {
            _modelBuilder.HasSequence("investmentSetting_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("InvestmentSetting");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("investmentSetting_seq");

            builder.Property(x => x.MaxInvestAmount).HasColumnType("decimal(18,2)");

            builder.Property(x => x.AmountPerPoint).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MaxInvestmentQueryMonths).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DefaultPointsForTicket).HasColumnType("decimal(8,2)").HasDefaultValue(0);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
