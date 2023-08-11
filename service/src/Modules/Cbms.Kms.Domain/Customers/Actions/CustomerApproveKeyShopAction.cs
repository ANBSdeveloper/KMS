using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerApproveKeyShopAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; set; }
        public CustomerApproveKeyShopAction(ILocalizationSource localizationSource)
        {
            LocalizationSource = localizationSource;
        }
    }
}