using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Cycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Budgets
{
    internal class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget>
    {
        private readonly ModelBuilder _modelBuilder;

        public BudgetEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            _modelBuilder.HasSequence("budget_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Budgets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("budget_seq");

            var branchNavigation = builder.Metadata.FindNavigation(nameof(Budget.Branches));
            branchNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var areaNavigation = builder.Metadata.FindNavigation(nameof(Budget.Areas));
            areaNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var zoneNavigation = builder.Metadata.FindNavigation(nameof(Budget.Zones));
            zoneNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Cycle>().WithMany().HasForeignKey(p => p.CycleId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}