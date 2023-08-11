using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.CustomerSalesItems;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.CustomerSalesItems
{
    internal class CustomerSalesItemEntityTypeConfiguration : IEntityTypeConfiguration<CustomerSalesItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public CustomerSalesItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<CustomerSalesItem> builder)
        {
            _modelBuilder.HasSequence("customer_sales_item_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("CustomerSalesItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("customer_sales_item_seq");

            builder.Property(x => x.QrCode).HasMaxLength(100).IsUnicode(false);

            builder.HasIndex(x => new { x.QrCode }).IsUnique();

            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<TicketInvestment>().WithMany().HasForeignKey(p => p.TicketInvestmentId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}