using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerRegisterAction : IEntityAction
    {
        public CustomerRegisterAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string userName, 
            string fullName,
            string phone,
            string password)
        {
            LocalizationSource = localizationSource;
            IocResolver = iocResolver;
            UserName = userName;
            FullName = fullName;
            Phone = phone;
            Password = password;
        }
        public ILocalizationSource LocalizationSource { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public string UserName { get; private set; }
        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
    }
}