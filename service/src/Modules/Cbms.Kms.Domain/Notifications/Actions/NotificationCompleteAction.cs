using Cbms.Dependency;
using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationCompleteAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public NotificationCompleteAction(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
    }
}
