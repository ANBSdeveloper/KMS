using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmInvestments
{
    internal class PosmInvestmentItemHistoryEntityTypeConfiguration : IEntityTypeConfiguration<PosmInvestmentItemHistory>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmInvestmentItemHistoryEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmInvestmentItemHistory> builder)
        {
            _modelBuilder.HasSequence("posm_investment_item_history_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("PosmInvestmentItemHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_investment_item_history_seq");

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}