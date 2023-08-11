using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Commands
{
    public class NotificationSendCommand : UpsertEntityCommand<NotificationSendDto, NotificationDto>
    {
        public NotificationSendCommand(NotificationSendDto data, string handleType) : base(data, handleType)
        {
        }
    }
}