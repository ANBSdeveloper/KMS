using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerValidateRecoveryPasswordAction : IEntityAction
    {
        public CustomerValidateRecoveryPasswordAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource)
        {
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
    }
}