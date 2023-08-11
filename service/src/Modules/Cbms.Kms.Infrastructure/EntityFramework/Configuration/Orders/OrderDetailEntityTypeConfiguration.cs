using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Orders
{
    internal class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        private readonly ModelBuilder _modelBuilder;

        public OrderDetailEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            _modelBuilder.HasSequence("order_detail_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("OrderDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("order_detail_seq");

            builder.Property(x => x.Quantity).HasColumnType("decimal(18,0)");
            builder.Property(x => x.LineAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.Points).HasColumnType("decimal(8,2)");
            builder.Property(x => x.AvailablePoints).HasColumnType("decimal(8,2)");
            builder.Property(x => x.UsedPoints).HasColumnType("decimal(8,2)");
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Api).IsUnicode(false).HasMaxLength(20);
            builder.Property(x => x.QrCode).IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.UnitName).IsUnicode(true).HasMaxLength(50);
            builder.Property(x => x.ProductName).IsUnicode(true).HasMaxLength(200);
            builder.Property(x => x.SpoonCode).IsUnicode(false).HasMaxLength(50);

            builder.HasIndex(x => x.SpoonCode).IsUnique();
            builder.HasIndex(x => x.QrCode);

            builder.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}