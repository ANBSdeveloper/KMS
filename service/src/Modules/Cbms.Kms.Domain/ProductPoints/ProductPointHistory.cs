using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Extensions;
using Cbms.Kms.Domain.ProductPoints.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.ProductPoints
{
    public class ProductPointHistory : AuditedAggregateRoot
    {
        public int ProductId { get; private set; }
        public decimal Points { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsActive { get; private set; }

        public ProductPointHistory()
        {
        }
        public static ProductPoint Create()
        {
            return new ProductPoint();
        }


        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case ProductPointUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(ProductPointUpsertAction action)
        {
            FromDate = action.FromDate.ToLocalTime().Date;
            ToDate = action.ToDate.ToLocalTime().EndOfDay();
            IsActive = action.IsActive;
            ProductId = action.ProductId;
            Points = action.Points;
        }
    }
}