using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Query
{
    public class NotificationGetUserList : EntityPagingResultQuery<NotificationUserListDto>
    {
        public bool? Unread { get; set; }
    }
}