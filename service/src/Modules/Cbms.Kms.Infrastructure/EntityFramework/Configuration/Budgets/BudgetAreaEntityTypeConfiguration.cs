using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Budgets
{
    internal class BudgetAreaEntityTypeConfiguration : IEntityTypeConfiguration<BudgetArea>
    {
        private readonly ModelBuilder _modelBuilder;

        public BudgetAreaEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<BudgetArea> builder)
        {
            _modelBuilder.HasSequence("budget_area_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("BudgetAreas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("budget_area_seq");

            builder.Property(x => x.AllocateAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.RemainAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.UsedAmount).HasColumnType("decimal(18,0)");

            builder.HasOne<Area>().WithMany().HasForeignKey(p => p.AreaId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}