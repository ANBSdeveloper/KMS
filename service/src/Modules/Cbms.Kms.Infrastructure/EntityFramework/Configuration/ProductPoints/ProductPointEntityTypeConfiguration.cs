using Cbms.Authorization.Users;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.ProductPoints
{
    internal class ProductPointEntityTypeConfiguration : IEntityTypeConfiguration<ProductPoint>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductPointEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<ProductPoint> builder)
        {
            _modelBuilder.HasSequence("product_point_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("ProductPoints");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("product_point_seq");

            builder.Property(x => x.Points).HasColumnType("decimal(8,2)");


            builder.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
          
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}