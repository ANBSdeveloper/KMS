using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketAcceptanceEntityTypeConfiguration : IEntityTypeConfiguration<TicketAcceptance>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketAcceptanceEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketAcceptance> builder)
        {
            _modelBuilder.HasSequence("ticket_acceptance_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketAcceptances");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_acceptance_seq");
          
            builder.Property(x => x.RemarkOfCompany).HasColumnType("decimal(10,2)").IsRequired(false);
            builder.Property(x => x.RemarkOfCustomerDevelopement).HasColumnType("decimal(10,2)").IsRequired(false);
            builder.Property(x => x.RemarkOfSales).HasColumnType("decimal(10,2)").IsRequired(false);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UpdateUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}