using Cbms.Authorization.Users;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketRewardItemEntityTypeConfiguration : IEntityTypeConfiguration<TicketRewardItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketRewardItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketRewardItem> builder)
        {
            _modelBuilder.HasSequence("ticket_reward_item_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("TicketRewardItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_reward_item_seq");
            builder.Property(x => x.Amount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.Price).HasColumnType("decimal(18,0)");

            builder.HasOne<RewardItem>().WithMany().HasForeignKey(p => p.RewardItemId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}