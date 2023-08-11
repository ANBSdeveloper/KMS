using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Budgets
{
    internal class BudgetZoneEntityTypeConfiguration : IEntityTypeConfiguration<BudgetZone>
    {
        private readonly ModelBuilder _modelBuilder;

        public BudgetZoneEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<BudgetZone> builder)
        {
            _modelBuilder.HasSequence("budget_zone_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("BudgetZones");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("budget_zone_seq");

            builder.Property(x => x.AllocateAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.RemainAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.UsedAmount).HasColumnType("decimal(18,0)");

            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}