using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationUserCreateAction : IEntityAction
    {
        public int UserId { get; private set; }

        public NotificationUserCreateAction(int userId)
        {
            UserId = userId;
        }
    }
}
