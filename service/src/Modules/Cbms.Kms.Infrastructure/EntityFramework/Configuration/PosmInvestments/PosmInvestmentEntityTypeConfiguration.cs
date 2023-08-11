using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.PosmInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cbms.Kms.Domain.CustomerLocations;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmInvestments
{
    internal class PosmInvestmentEntityTypeConfiguration : IEntityTypeConfiguration<PosmInvestment>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmInvestmentEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmInvestment> builder)
        {
            _modelBuilder.HasSequence("posm_investment_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("PosmInvestments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_investment_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.CommitmentAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.InvestmentAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.CurrentSalesAmount).HasColumnType("decimal(18,0)");

            builder.Property(x => x.ShopPanelPhoto1).IsUnicode(false);
            builder.Property(x => x.ShopPanelPhoto2).IsUnicode(false);
            builder.Property(x => x.ShopPanelPhoto3).IsUnicode(false);
            builder.Property(x => x.ShopPanelPhoto4).IsUnicode(false);

            builder.Property(x => x.VisibilityPhoto1).IsUnicode(false);
            builder.Property(x => x.VisibilityPhoto2).IsUnicode(false);
            builder.Property(x => x.VisibilityPhoto3).IsUnicode(false);
            builder.Property(x => x.VisibilityPhoto4).IsUnicode(false);

            builder.Property(x => x.VisibilityCompetitorPhoto1).IsUnicode(false);
            builder.Property(x => x.VisibilityCompetitorPhoto2).IsUnicode(false);
            builder.Property(x => x.VisibilityCompetitorPhoto3).IsUnicode(false);
            builder.Property(x => x.VisibilityCompetitorPhoto4).IsUnicode(false);


            var itemNavigation = builder.Metadata.FindNavigation(nameof(PosmInvestment.Items));
            itemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var salesCommitmentNavigation = builder.Metadata.FindNavigation(nameof(PosmInvestment.SalesCommitments));
            salesCommitmentNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(p => new { p.Status, p.CreationTime });
            builder.HasIndex(p => new { p.Code });

            builder.HasOne<Budget>().WithMany().HasForeignKey(p => p.BudgetId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Cycle>().WithMany().HasForeignKey(p => p.CycleId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Staff>().WithMany().HasForeignKey(p => p.RegisterStaffId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<CustomerLocation>().WithMany().HasForeignKey(p => p.CustomerLocationId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}