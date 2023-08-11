using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Geography.Districts;
using Cbms.Kms.Domain.Geography.Wards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Geography
{
    internal class WardEntityTypeConfiguration : IEntityTypeConfiguration<Ward>
    {
        private readonly ModelBuilder _modelBuilder;

        public WardEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Ward> builder)
        {
            _modelBuilder.HasSequence("ward_seq").IncrementsBy(100).StartsAt(1);

            builder.ToTable("Wards");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ward_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.UpdateDate).IsRequired(true);

            builder.HasOne<District>().WithMany().HasForeignKey(p => p.DistrictId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
