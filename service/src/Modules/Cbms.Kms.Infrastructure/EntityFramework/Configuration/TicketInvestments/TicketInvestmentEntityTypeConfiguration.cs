using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketInvestmentEntityTypeConfiguration : IEntityTypeConfiguration<TicketInvestment>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketInvestmentEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketInvestment> builder)
        {
            _modelBuilder.HasSequence("ticket_investment_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketInvestments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_investment_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.PointsForTicket).HasColumnType("decimal(18,2)");
            builder.Property(x => x.SalesPlanAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.CommitmentAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.ActualSalesAmount).HasColumnType("decimal(18,0)");

            var rewardItemNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.RewardItems));
            rewardItemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var materialNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.Materials));
            materialNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var updateNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.Progresses));
            updateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var ticketNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.Tickets));
            ticketNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var consumerRewardNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.ConsumerRewards));
            consumerRewardNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var salesCommitmentNavigation = builder.Metadata.FindNavigation(nameof(TicketInvestment.SalesCommitments));
            salesCommitmentNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(p => new { p.Status, p.CreationTime });
            builder.HasIndex(p => new { p.Code });

            builder.HasOne(p => p.TicketFinalSettlement).WithOne();
            builder.HasOne(p => p.TicketAcceptance).WithOne();
            builder.HasOne(p => p.TicketOperation).WithOne();

            builder.HasOne<Budget>().WithMany().HasForeignKey(p => p.BudgetId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Cycle>().WithMany().HasForeignKey(p => p.CycleId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<RewardPackage>().WithMany().HasForeignKey(p => p.RewardPackageId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Staff>().WithMany().HasForeignKey(p => p.RegisterStaffId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}