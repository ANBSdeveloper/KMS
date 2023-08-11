using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class ConsumerCreateAction : IEntityAction
    {
        public ConsumerCreateAction(
            string phone, 
            string name)
        {
            Phone = phone;
            Name = name;
        }

        public string Phone { get; private set; }
        public string Name { get; private set; }
    }
}