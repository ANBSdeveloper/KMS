using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.ProductPoints.Actions
{
    public class ProductPointUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int ProductId { get; private set; }
        public decimal Points { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsImport { get; private set; }
        public ProductPointUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int productId,
            decimal points,
            DateTime fromDate,
            DateTime toDate,
            bool isActive,
            bool isImport)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            ProductId = productId;
            Points = points;
            FromDate = fromDate;
            ToDate = toDate;
            IsActive = isActive;
            IsImport = isImport;
        }
    }
}