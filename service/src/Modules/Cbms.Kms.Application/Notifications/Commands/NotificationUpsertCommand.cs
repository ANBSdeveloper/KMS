using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Commands
{
    public class NotificationUpsertCommand : UpsertEntityCommand<NotificationUpsertDto, NotificationDto>
    {
        public NotificationUpsertCommand(NotificationUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}