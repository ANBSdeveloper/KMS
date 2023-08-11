using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketSalesCommitmentEntityTypeConfiguration : IEntityTypeConfiguration<TicketSalesCommitment>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketSalesCommitmentEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketSalesCommitment> builder)
        {
            _modelBuilder.HasSequence("ticket_sales_commitment_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("TicketSalesCommitments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_sales_commitment_seq");

            builder.Property(x => x.Amount).HasColumnType("decimal(18,0)");

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}