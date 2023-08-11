using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerCheckOtpAction : IEntityAction
    {
        public CustomerCheckOtpAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource, 
            string otpCode)
        {
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
            OtpCode = otpCode;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public string OtpCode { get; private set; }
    }
}