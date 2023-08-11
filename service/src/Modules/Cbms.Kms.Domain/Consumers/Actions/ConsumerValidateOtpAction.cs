using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class ConsumerValidateOtpAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }

        public ConsumerValidateOtpAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource, 
            string otpCode)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            OtpCode = otpCode;
        }

        public string OtpCode { get; private set; }
    }
}