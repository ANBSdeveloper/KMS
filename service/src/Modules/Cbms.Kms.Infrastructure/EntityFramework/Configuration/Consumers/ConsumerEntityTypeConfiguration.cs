using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Consumers
{
    internal class ConsumerEntityTypeConfiguration : IEntityTypeConfiguration<Consumer>
    {
        private readonly ModelBuilder _modelBuilder;

        public ConsumerEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Consumer> builder)
        {
            _modelBuilder.HasSequence("consumer_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Consumers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("consumer_seq");

            builder.Property(x => x.Phone).HasMaxLength(20).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true);
            builder.Property(x => x.OtpCode).HasMaxLength(10).IsUnicode(false);
            builder.Property(x => x.OtpTime).IsRequired(false);

            builder.HasIndex(x => x.Phone).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}