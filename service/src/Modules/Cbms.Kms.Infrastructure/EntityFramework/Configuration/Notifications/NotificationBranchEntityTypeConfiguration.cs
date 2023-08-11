using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.RewardPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Notifications
{
    internal class NotificationBranchEntityTypeConfiguration : IEntityTypeConfiguration<NotificationBranch>
    {
        private readonly ModelBuilder _modelBuilder;

        public NotificationBranchEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<NotificationBranch> builder)
        {
            _modelBuilder.HasSequence("notification_branch_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("NotificationBranches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("notification_branch_seq");

            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}