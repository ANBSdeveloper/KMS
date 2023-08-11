using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerValidateActivationKeyShopAction : IEntityAction
    {
        public CustomerValidateActivationKeyShopAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string authCode,
            string mobilePhone,
            string name,
            DateTime birthDay)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            AuthCode = authCode;
            MobilePhone = mobilePhone;
            Name = name;
            Birthday = birthDay;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public string AuthCode { get; private set; }
        public string MobilePhone { get; private set; }
        public string Name { get; private set; }
        public DateTime Birthday { get; private set; }
    }
}