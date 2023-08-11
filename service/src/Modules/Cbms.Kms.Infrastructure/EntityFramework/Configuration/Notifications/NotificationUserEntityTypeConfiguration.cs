using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Notifications
{
    internal class NotificationUserEntityTypeConfiguration : IEntityTypeConfiguration<NotificationUser>
    {
        private readonly ModelBuilder _modelBuilder;

        public NotificationUserEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            _modelBuilder.HasSequence("notification_user_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("NotificationUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("notification_user_seq");

            builder.Property(x => x.ViewDate).IsRequired(false);
          
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}