using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.RewardPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.RewardPackages
{
    internal class RewardBranchEntityTypeConfiguration : IEntityTypeConfiguration<RewardBranch>
    {
        private readonly ModelBuilder _modelBuilder;

        public RewardBranchEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<RewardBranch> builder)
        {
            _modelBuilder.HasSequence("reward_branch_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("RewardBranches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("reward_branch_seq");

            builder.Property(x => x.RewardPackageId).IsRequired(true);
            builder.Property(x => x.BranchId).IsRequired(true);
            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}