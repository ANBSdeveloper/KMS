using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.CustomerSales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.CustomerSales
{
    internal class CustomerSaleEntityTypeConfiguration : IEntityTypeConfiguration<CustomerSale>
    {
        private readonly ModelBuilder _modelBuilder;

        public CustomerSaleEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<CustomerSale> builder)
        {
            _modelBuilder.HasSequence("customer_sale_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("CustomerSales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("customer_sale_seq");

            builder.Property(x => x.Year).HasMaxLength(4).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Month).HasMaxLength(2).IsUnicode(false).IsRequired(true);
            builder.Property(x => x.YearMonth).HasMaxLength(6).IsUnicode(false).IsRequired(true);
            builder.Property(x => x.Amount).HasColumnType("decimal(18,0)");

            builder.Property(x => x.CustomerId).IsRequired(true);

            builder.HasIndex(x => new { x.CustomerId, x.YearMonth }).IsUnique();

            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}