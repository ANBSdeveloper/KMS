using Cbms.Authorization.Users;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketConsumerRewardEntityTypeConfiguration : IEntityTypeConfiguration<TicketConsumerReward>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketConsumerRewardEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketConsumerReward> builder)
        {
            _modelBuilder.HasSequence("ticket_customer_reward_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketConsumerRewards");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_customer_reward_seq");

            var rewardItemNavigation = builder.Metadata.FindNavigation(nameof(TicketConsumerReward.Details));
            rewardItemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<RewardItem>().WithMany().HasForeignKey(p => p.RewardItemId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
           
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}