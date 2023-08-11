using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.ProductPrices.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.ProductPrices
{
    public class ProductPrice : AuditedAggregateRoot
    {
        public int ProductId { get; private set; }
        public int? CustomerId { get; private set; }
        public int? SalesOrgId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime FromDate { get; private  set; }
        public DateTime? ToDate { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public decimal Price { get; private set; }
        public decimal PackagePrice { get; private set; }
   
        public ProductPrice()
        {
        }


        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case ProductPriceUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(ProductPriceUpsertAction action)
        {
            FromDate = action.FromDate;
            ToDate = action.ToDate;
            Id = action.Id;
            SalesOrgId = action.SalesOrgId;
            CustomerId = action.CustomerId;
            UpdateDate = action.UpdateDate;
            Price = action.Price;
            PackagePrice = action.PackagePrice;
            IsActive = action.IsActive;
            ProductId = action.ProductId;
        }
    }
}