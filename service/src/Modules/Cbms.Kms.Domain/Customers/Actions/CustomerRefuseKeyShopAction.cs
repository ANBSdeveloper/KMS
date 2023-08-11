using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerRefuseKeyShopAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; set; }
        public CustomerRefuseKeyShopAction(ILocalizationSource localizationSource)
        {
            LocalizationSource = localizationSource;
        }
    }
}