using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Notifications
{
    internal class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        private readonly ModelBuilder _modelBuilder;

        public NotificationEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            _modelBuilder.HasSequence("notification_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("notification_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired(true);
            builder.Property(x => x.ShortContent).HasMaxLength(500).IsRequired(true);
            builder.Property(x => x.Content).IsRequired(true);

            var notificationUserNavigation = builder.Metadata.FindNavigation(nameof(Notification.NotificationUsers));
            notificationUserNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var notificationBranchNavigation = builder.Metadata.FindNavigation(nameof(Notification.NotificationBranches));
            notificationBranchNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}