using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Budgets
{
    internal class BudgetBranchEntityTypeConfiguration : IEntityTypeConfiguration<BudgetBranch>
    {
        private readonly ModelBuilder _modelBuilder;

        public BudgetBranchEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<BudgetBranch> builder)
        {
            _modelBuilder.HasSequence("budget_branch_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("BudgetBranches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("budget_branch_seq");

            builder.Property(x => x.AllocateAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.RemainAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.UsedAmount).HasColumnType("decimal(18,0)");

            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}