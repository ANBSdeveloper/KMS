using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmItems
{
    internal class PosmCatalogEntityTypeConfiguration : IEntityTypeConfiguration<PosmCatalog>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmCatalogEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmCatalog> builder)
        {
            _modelBuilder.HasSequence("posm_catalog_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("PosmCatalogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_catalog_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true).IsUnicode(true);
            builder.Property(x => x.Link).HasMaxLength(1000).IsUnicode(true);
          
            builder.HasIndex(p => new { p.PosmItemId, p.Code }).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}