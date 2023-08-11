using Cbms.Authorization.Users;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketProgressEntityTypeConfiguration : IEntityTypeConfiguration<TicketProgress>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketProgressEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketProgress> builder)
        {
            _modelBuilder.HasSequence("ticket_progress_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketProgresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_progress_seq");

            var rewardItemNavigation = builder.Metadata.FindNavigation(nameof(TicketProgress.RewardItems));
            rewardItemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var materialNavigation = builder.Metadata.FindNavigation(nameof(TicketProgress.Materials));
            materialNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UpdateUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}