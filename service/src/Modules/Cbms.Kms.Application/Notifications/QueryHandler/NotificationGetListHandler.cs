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
    public class NotificationGetListHandler : QueryHandlerBase, IRequestHandler<NotificationGetList, PagingResult<NotificationListDto>>
    {
        private readonly AppDbContext _dbContext;

        public NotificationGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<NotificationListDto>> Handle(NotificationGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from notification in _dbContext.Notifications
                        where !notification.IsSystem
                        select new NotificationListDto()
                        {
                            Code = notification.Code,
                            Description = notification.Description,
                            ShortContent = notification.ShortContent,
                            Content = notification.Content,
                            Status = (int)notification.Status,
                            Id = notification.Id,
                            ObjectType = (int)notification.ObjectType,
                            CreationTime = notification.CreationTime,
                            CreatorUserId = notification.CreatorUserId,
                            LastModificationTime = notification.LastModificationTime,
                            LastModifierUserId = notification.LastModifierUserId
                        };

            query = query
                .WhereIf(request.Status.HasValue, x => x.Status == request.Status.Value)
                .WhereIf(request.ObjectType.HasValue, x => x.ObjectType == request.ObjectType.Value)
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
            return new PagingResult<NotificationListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}