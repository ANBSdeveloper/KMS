using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketConsumerRewardDetailEntityTypeConfiguration : IEntityTypeConfiguration<TicketConsumerRewardDetail>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketConsumerRewardDetailEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketConsumerRewardDetail> builder)
        {
            _modelBuilder.HasSequence("ticket_customer_reward_detail_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("TicketConsumerRewardDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_customer_reward_detail_seq");

            builder.HasOne<Ticket>().WithMany().HasForeignKey(p => p.TicketId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}