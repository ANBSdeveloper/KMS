using AutoMapper;
using Cbms.Kms.Domain.Notifications;

namespace Cbms.Kms.Application.Notifications.Dto
{
    [AutoMap(typeof(Notification))]
    public class NotificationListDto : NotificationBaseDto
    {
    }
}