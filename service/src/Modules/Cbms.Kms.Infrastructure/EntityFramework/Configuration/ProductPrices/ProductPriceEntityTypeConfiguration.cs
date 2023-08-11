using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.ProductPrices;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.SalesOrgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.ProductPrices
{
    internal class ProductPriceEntityTypeConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductPriceEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {

            builder.ToTable("ProductPrices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.CustomerId).IsRequired(false);
            builder.Property(x => x.SalesOrgId).IsRequired(false);

            builder.Property(x => x.ToDate).IsRequired(false);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.PackagePrice).HasColumnType("decimal(18,2)");

            builder.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<SalesOrg>().WithMany().HasForeignKey(p => p.SalesOrgId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}