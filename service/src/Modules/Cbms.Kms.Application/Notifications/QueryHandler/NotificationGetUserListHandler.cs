using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Application.Notifications.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications.QueryHandler
{
    public class NotificationGetUserListHandler : QueryHandlerBase, IRequestHandler<NotificationGetUserList, PagingResult<NotificationUserListDto>>
    {
        private readonly AppDbContext _dbContext;

        public NotificationGetUserListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<NotificationUserListDto>> Handle(NotificationGetUserList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from notification in _dbContext.Notifications
                        join notificationUser in _dbContext.NotificationUsers on notification.Id equals notificationUser.NotificationId
                        where notificationUser.UserId == Session.UserId
                        select new NotificationUserListDto()
                        {
                            Code = notification.Code,
                            Description = notification.Description,
                            ShortContent = notification.ShortContent,
                            Content = notification.Content,
                            Id = notification.Id,
                            ViewDate = notificationUser.ViewDate,
                            CreationDate = notification.CreationTime,
                        };

            query = query
                .WhereIf(request.Unread.HasValue, p => request.Unread.Value ? p.ViewDate == null : p.ViewDate != null)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                    x.ShortContent.Contains(keyword) || x.Description.Contains(keyword));

            int totalCount = query.Count();

            query = query.SortFromString(request.Sort);

            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<NotificationUserListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}