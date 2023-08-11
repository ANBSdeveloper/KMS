using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Notifications.Query
{
    public class NotificationUnreadCountGet : QueryBase, IRequest<int>
    {
       
    }
}