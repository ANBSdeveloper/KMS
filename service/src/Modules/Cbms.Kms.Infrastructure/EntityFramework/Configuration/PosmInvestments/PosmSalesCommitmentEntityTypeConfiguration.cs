using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmInvestments
{
    internal class PosmSalesCommitmentEntityTypeConfiguration : IEntityTypeConfiguration<PosmSalesCommitment>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmSalesCommitmentEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmSalesCommitment> builder)
        {
            _modelBuilder.HasSequence("posm_sales_commitment_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("PosmSalesCommitments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_sales_commitment_seq");

            builder.Property(x => x.Amount).HasColumnType("decimal(18,0)");

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}