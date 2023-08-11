using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Extensions;
using Cbms.Kms.Domain.ProductPoints.Actions;
using Cbms.Kms.Domain.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.ProductPoints
{
    public class ProductPoint : AuditedAggregateRoot
    {
        public int ProductId { get; private set; }
        public decimal Points { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsActive { get; private set; }

        public ProductPoint()
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
            var productPointRepository = action.IocResolver.Resolve<IRepository<ProductPoint, int>>();

            FromDate = action.FromDate.ToLocalTime().Date;
            ToDate = action.ToDate.ToLocalTime().EndOfDay();
            IsActive = action.IsActive;
            ProductId = action.ProductId;
            Points = action.Points;

            var conflictProduct = productPointRepository
                .GetAll()
                .Where(p =>
                    p.ProductId == ProductId &&
                    p.Id != Id &&
                    (
                        (p.FromDate <= FromDate && p.ToDate >= FromDate) ||
                        (p.FromDate <= ToDate && p.ToDate >= ToDate)
                    )
                ).FirstOrDefault();

            if (conflictProduct != null)
            {
                var productRepository = action.IocResolver.Resolve<IRepository<Product, int>>();
                var product = await productRepository.GetAsync(ProductId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode(
                    "Product.ConflictPointTime",
                    product.Code,
                    FromDate.ToShortDateString(),
                    ToDate.ToShortDateString(),
                    conflictProduct.FromDate.ToShortDateString(),
                    conflictProduct.ToDate.ToShortDateString()
                ).Build();
            }
        }
    }
}