using Cbms.Dependency;
using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerUpdateEfficientAction : IEntityAction
    {
        public CustomerUpdateEfficientAction(
            IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
        public IIocResolver IocResolver { get; private set; }
    }
}