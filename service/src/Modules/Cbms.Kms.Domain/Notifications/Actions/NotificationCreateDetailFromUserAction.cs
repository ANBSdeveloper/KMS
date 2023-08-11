using Cbms.Dependency;
using Cbms.Domain.Entities;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationCreateDetailFromUserAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public List<int> Users { get; private set; }
        public NotificationCreateDetailFromUserAction(IIocResolver iocResolver, List<int> users)
        {
            IocResolver = iocResolver;
            Users = users;
        }
    }
}
