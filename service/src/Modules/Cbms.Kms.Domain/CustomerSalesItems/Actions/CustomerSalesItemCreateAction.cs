using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.CustomerSalesItems.Actions
{
    public class CustomerSalesItemCreateAction : IEntityAction
    {
        public CustomerSalesItemCreateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int customerId,
            int productId,
            string qrCode
        )
        {
            CustomerId = customerId;
            ProductId = productId;
            QrCode = (qrCode ?? "").Trim();
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
        }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public string QrCode { get; private set; }
    }
}