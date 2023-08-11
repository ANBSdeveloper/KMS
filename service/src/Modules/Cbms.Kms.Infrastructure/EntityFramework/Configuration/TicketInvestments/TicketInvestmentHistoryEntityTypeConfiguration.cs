using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketInvestmentHistoryEntityTypeConfiguration : IEntityTypeConfiguration<TicketInvestmentHistory>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketInvestmentHistoryEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketInvestmentHistory> builder)
        {
            _modelBuilder.HasSequence("ticket_investment_history_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketInvestmentHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_investment_history_seq");

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}