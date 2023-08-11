using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class ConsumerSendOtpAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }

        public ConsumerSendOtpAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource, 
            string phone)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Phone = phone;
        }

        public string Phone { get; private set; }
    }
}