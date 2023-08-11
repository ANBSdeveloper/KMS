using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Orders
{
    internal class OrderTicketEntityTypeConfiguration : IEntityTypeConfiguration<OrderTicket>
    {
        private readonly ModelBuilder _modelBuilder;

        public OrderTicketEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<OrderTicket> builder)
        {
            _modelBuilder.HasSequence("order_ticket_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("OrderTickets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("order_ticket_seq");
            builder.Property(x => x.QrCode).IsUnicode(false).HasMaxLength(30);

            builder.HasIndex(x => x.QrCode);

            builder.HasOne<Ticket>().WithMany().HasForeignKey(p => p.TicketId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}