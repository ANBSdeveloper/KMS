using Cbms.Authorization.Users;
using Cbms.Kms.Domain.RewardPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.RewardPackages
{
    internal class RewardPackageEntityTypeConfiguration : IEntityTypeConfiguration<RewardPackage>
    {
        private readonly ModelBuilder _modelBuilder;

        public RewardPackageEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<RewardPackage> builder)
        {
            _modelBuilder.HasSequence("reward_package_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("RewardPackages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("reward_package_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true).IsUnicode(true);
            builder.Property(x => x.FromDate).IsRequired(true);
            builder.Property(x => x.ToDate).IsRequired(true);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.TotalTickets).HasColumnType("decimal(18,0)");

            builder.HasIndex(p => new { p.Code }).IsUnique();

            var rewardItemsNavigation = builder.Metadata.FindNavigation(nameof(RewardPackage.RewardItems));
            rewardItemsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var rewardBranchesNavigation = builder.Metadata.FindNavigation(nameof(RewardPackage.RewardBranches));
            rewardBranchesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}