using Cbms.Dependency;
using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationGenerateUserAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public NotificationGenerateUserAction(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
    }
}
