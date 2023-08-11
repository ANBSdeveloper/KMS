using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketEntityTypeConfiguration : IEntityTypeConfiguration<Ticket>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            _modelBuilder.HasSequence("ticket_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("Tickets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_seq");

            builder.Property(x => x.Code).IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.ConsumerName).HasMaxLength(200);
            builder.Property(x => x.ConsumerPhone).HasMaxLength(2000);
            builder.Property(x => x.IssueDate).IsRequired(false);
            builder.Property(x => x.PrintDate).IsRequired(false);
            builder.Property(x => x.LastPrintUserId).IsRequired(false);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastPrintUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}