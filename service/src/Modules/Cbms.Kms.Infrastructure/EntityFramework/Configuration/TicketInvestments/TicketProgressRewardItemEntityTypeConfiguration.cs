using Cbms.Authorization.Users;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketProgressRewardItemEntityTypeConfiguration : IEntityTypeConfiguration<TicketProgressRewardItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketProgressRewardItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketProgressRewardItem> builder)
        {
            _modelBuilder.HasSequence("ticket_progress_reward_item_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketProgressRewardItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_progress_reward_item_seq");

            builder.HasOne<RewardItem>().WithMany().HasForeignKey(p => p.RewardItemId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}