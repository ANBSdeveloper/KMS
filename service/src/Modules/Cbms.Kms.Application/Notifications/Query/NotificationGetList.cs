using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Query
{
    public class NotificationGetList : EntityPagingResultQuery<NotificationListDto>
    {
        public int? Status { get; set; }
        public int? ObjectType { get; set; }
    }
}