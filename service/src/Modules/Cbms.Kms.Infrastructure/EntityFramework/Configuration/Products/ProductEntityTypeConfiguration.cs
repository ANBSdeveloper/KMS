using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Brands;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.SubProductClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Products
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            _modelBuilder.HasSequence("product_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("product_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Unit).HasMaxLength(50);
            builder.Property(x => x.CaseUnit).HasMaxLength(50);

            builder.Property(x => x.BrandId).IsRequired(false);
            builder.Property(x => x.ProductClassId).IsRequired(false);

            builder.HasIndex(p => new { p.Code }).IsUnique();

            builder.HasOne<Brand>().WithMany().HasForeignKey(p => p.BrandId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ProductClass>().WithMany().HasForeignKey(p => p.ProductClassId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<SubProductClass>().WithMany().HasForeignKey(p => p.SubProductClassId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

        }
    }
}