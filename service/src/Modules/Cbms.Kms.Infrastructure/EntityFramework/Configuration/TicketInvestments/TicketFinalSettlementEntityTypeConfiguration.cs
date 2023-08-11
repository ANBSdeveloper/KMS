using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketFinalSettlementEntityTypeConfiguration : IEntityTypeConfiguration<TicketFinalSettlement>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketFinalSettlementEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketFinalSettlement> builder)
        {
            _modelBuilder.HasSequence("ticket_final_settlement_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketFinalSettlements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_final_settlement_seq");
          
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UpdateUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.DecideUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}