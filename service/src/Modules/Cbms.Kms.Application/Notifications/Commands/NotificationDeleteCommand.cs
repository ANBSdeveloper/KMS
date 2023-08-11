using Cbms.Mediator;

namespace Cbms.Kms.Application.Notifications.Commands
{
    public class NotificationDeleteCommand : DeleteEntityCommand
    {
        public NotificationDeleteCommand(int id) : base(id)
        {
        }
    }
}