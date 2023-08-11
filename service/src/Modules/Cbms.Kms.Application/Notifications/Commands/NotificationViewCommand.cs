using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Notifications.Commands
{
    public class NotificationViewCommand : QueryBase, IRequest
    {
        public int Id { get; private set; }
        public NotificationViewCommand(int id)
        {
            Id = id;
        }
    }
}