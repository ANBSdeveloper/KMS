using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Query
{
    public class NotificationGet : EntityQuery<NotificationDto>
    {
        public NotificationGet(int id) : base(id)
        {
        }
    }
}