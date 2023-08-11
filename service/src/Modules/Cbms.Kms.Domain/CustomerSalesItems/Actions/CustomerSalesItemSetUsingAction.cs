using Cbms.Dependency;
using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.CustomerSalesItems.Actions
{
    public class CustomerSalesItemSetUsingAction : IEntityAction
    {
        public CustomerSalesItemSetUsingAction(
            IIocResolver iocResolver
        )
        {
            IocResolver = iocResolver;
        }
        public IIocResolver IocResolver { get; private set; }
    }
}