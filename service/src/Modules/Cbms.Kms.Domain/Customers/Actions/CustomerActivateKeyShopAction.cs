using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerActivateKeyShopAction : IEntityAction
    {
        public CustomerActivateKeyShopAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string otpCode, 
            string password)
        {
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
            OtpCode = otpCode;
            Password = password;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public string OtpCode { get; private set; }
        public string Password { get; private set; }
    }
}