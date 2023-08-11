using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketOperationEntityTypeConfiguration : IEntityTypeConfiguration<TicketOperation>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketOperationEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketOperation> builder)
        {
            _modelBuilder.HasSequence("ticket_operation_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketOperations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_operation_seq");

            builder.Property(x => x.StockQuantity).HasColumnType("decimal(18,0)");
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UpdateUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}