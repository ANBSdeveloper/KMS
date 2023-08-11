using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.Branches;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Orders
{
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        private readonly ModelBuilder _modelBuilder;

        public OrderEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            _modelBuilder.HasSequence("order_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("order_seq");

            builder.Property(x => x.OrderNumber).HasMaxLength(30).IsUnicode(false);
            builder.Property(x => x.ConsumerPhone).HasMaxLength(20).IsUnicode(false);
            builder.Property(x => x.ConsumerName).HasMaxLength(200).IsUnicode(true);
            builder.Property(x => x.TotalQuantity).HasColumnType("decimal(18,0)");
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,0)");
            builder.Property(x => x.TotalPoints).HasColumnType("decimal(8,2)");
            builder.Property(x => x.TotalAvailablePoints).HasColumnType("decimal(8,2)");
            builder.Property(x => x.TotalUsedPoints).HasColumnType("decimal(8,2)");

            var orderDetailNavigation = builder.Metadata.FindNavigation(nameof(Order.OrderDetails));
            orderDetailNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var orderTicketNavigation = builder.Metadata.FindNavigation(nameof(Order.OrderTickets));
            orderTicketNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(p => new { p.OrderNumber }).IsUnique();
            builder.HasIndex(p => new { p.OrderDate, p.TotalAvailablePoints });
            builder.HasIndex(p => new { p.ConsumerPhone, p.TotalAvailablePoints });

            builder.HasOne<TicketInvestment>().WithMany().HasForeignKey(p => p.TicketInvestmentId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Customer>().WithMany().HasForeignKey(p => p.CustomerId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
          
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}