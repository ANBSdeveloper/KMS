using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerRegisterKeyShopAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; private set; }
        public int StaffId  { get; private set; }
        public CustomerRegisterKeyShopAction(ILocalizationSource localizationSource, int staffId)
        {
            LocalizationSource = localizationSource;
            StaffId = staffId;
        }
    }
}