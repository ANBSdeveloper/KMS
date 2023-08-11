using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.ProductPrices.Actions
{
    public class ProductPriceUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public bool IsActive { get; private set; }
        public decimal Price { get; private set; }
        public decimal PackagePrice { get; private set; }
        public int? CustomerId { get; private set; }
        public int? SalesOrgId { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime? ToDate { get; private set; }

        public ProductPriceUpsertAction(
            int id,
            int productId,
            bool isActive,
            decimal price,
            decimal packagePrice,
            int? customerId,
            int? salesOrgId,
            DateTime updateDate,
            DateTime fromDate,
            DateTime? toDate)
        {
            Id = id;
            ProductId = productId;
            IsActive = isActive;
            Price = price;
            PackagePrice = packagePrice;
            CustomerId = customerId;
            SalesOrgId = salesOrgId;
            UpdateDate = updateDate;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}