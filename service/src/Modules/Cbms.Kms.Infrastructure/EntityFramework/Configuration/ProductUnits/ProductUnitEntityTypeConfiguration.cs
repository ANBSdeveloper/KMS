using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.ProductUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.ProductUnits
{
    internal class ProductUnitEntityTypeConfiguration : IEntityTypeConfiguration<ProductUnit>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductUnitEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<ProductUnit> builder)
        {
            _modelBuilder.HasSequence("product_unit_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("ProductUnits");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseHiLo("product_unit_seq");
            builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}