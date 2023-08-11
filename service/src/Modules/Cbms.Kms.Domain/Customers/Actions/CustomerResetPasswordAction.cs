using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerResetPasswordAction : IEntityAction
    {
        public CustomerResetPasswordAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource, 
            string otpCode,
            string newPassword)
        {
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
            NewPassword = newPassword;
            OtpCode = otpCode;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public string NewPassword { get; private set; }
        public string OtpCode { get; private set; }
    }
}