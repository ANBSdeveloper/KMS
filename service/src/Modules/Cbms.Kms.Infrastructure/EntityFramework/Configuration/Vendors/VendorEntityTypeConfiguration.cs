using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Vendors;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Vendors
{
    internal class VendorEntityTypeConfiguration : IEntityTypeConfiguration<Vendor>
    {
        private readonly ModelBuilder _modelBuilder;

        public VendorEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            _modelBuilder.HasSequence("vendor_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Vendors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("vendor_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Address).HasMaxLength(2000).IsUnicode(true);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.TaxReg).HasMaxLength(20);
            builder.Property(x => x.Representative).HasMaxLength(500).IsUnicode(true);

            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
