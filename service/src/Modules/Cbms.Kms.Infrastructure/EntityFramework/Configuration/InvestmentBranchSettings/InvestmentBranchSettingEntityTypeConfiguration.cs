using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Infrastructure
{
    internal class InvestmentBranchSettingEntityTypeConfiguration : IEntityTypeConfiguration<InvestmentBranchSetting>
    {
        private readonly ModelBuilder _modelBuilder;

        public InvestmentBranchSettingEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<InvestmentBranchSetting> builder)
        {
            _modelBuilder.HasSequence("investment_branch_setting_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("InvestmentBranchSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("investment_branch_setting_seq");

            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
