using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmPrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmItems
{
    internal class PosmPriceHeaderEntityTypeConfiguration : IEntityTypeConfiguration<PosmPriceHeader>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmPriceHeaderEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmPriceHeader> builder)
        {
            _modelBuilder.HasSequence("posm_price_header_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("PosmPriceHeaders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_price_header_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true).IsUnicode(true);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasIndex(p => new { p.Code }).IsUnique();

            var detailNavigation = builder.Metadata.FindNavigation(nameof(PosmPriceHeader.PosmPriceDetails));
            detailNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}