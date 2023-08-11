using Cbms.Kms.Application.Notifications.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.QueryHandler
{
    public class NotificationUnreadCountGetHandler : QueryHandlerBase, IRequestHandler<NotificationUnreadCountGet, int>
    {
        private readonly AppDbContext _dbContext;

        public NotificationUnreadCountGetHandler(IRequestSupplement supplement,  AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(NotificationUnreadCountGet request, CancellationToken cancellationToken)
        {
            var query = (from notification in _dbContext.Notifications
                         join notificationUser in _dbContext.NotificationUsers on notification.Id equals notificationUser.NotificationId
                         where notificationUser.UserId == Session.UserId && notificationUser.ViewDate == null
                         select notification);

            return await query.CountAsync();
        }
    }
}