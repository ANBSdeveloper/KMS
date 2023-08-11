using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Channels
{
    public class ChannelEntityTypeConfiguration : IEntityTypeConfiguration<Channel>
    {
        private readonly ModelBuilder _modelBuilder;

        public ChannelEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            _modelBuilder.HasSequence("channel_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("Channels");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("channel_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.Property(x => x.SalesOrgId).HasDefaultValue(0);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}