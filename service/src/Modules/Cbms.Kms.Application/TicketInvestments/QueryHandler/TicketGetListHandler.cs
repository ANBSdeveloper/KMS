using Cbms.Collections.Extensions;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandler
{
    public class TicketGetListHandler : QueryHandlerBase, IRequestHandler<TicketGetList, PagingResult<TicketDto>>
    {
        private readonly AppDbContext _dbContext;

        public TicketGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<TicketDto>> Handle(TicketGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from p in _dbContext.Tickets
                        where p.TicketInvestmentId == request.TicketInvestmentId
                        select new TicketDto()
                        {
                            Code = p.Code,
                            ConsumerPhone = p.ConsumerPhone,
                            ConsumerName = p.ConsumerName,
                            Id = p.Id,
                            IssueDate = p.IssueDate,
                            LastPrintUserId = p.LastPrintUserId,
                            PrintCount = p.PrintCount,
                            PrintDate = p.PrintDate
                        };

            query = query.WhereIf(!string.IsNullOrEmpty(request.Keyword),
                p => p.Code.Contains(keyword) ||
                p.ConsumerName.Contains(keyword) ||
                p.ConsumerPhone.Contains(keyword));

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
            return new PagingResult<TicketDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}