using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Zones
{
    internal class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<Zone>
    {
        private readonly ModelBuilder _modelBuilder;

        public ZoneEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            _modelBuilder.HasSequence("zone_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("Zones");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("zone_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.Property(x => x.SalesOrgId).HasDefaultValue(0);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}