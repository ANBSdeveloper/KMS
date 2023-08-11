

using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cbms.Mediator.Query.Pagination;
using Cbms.Linq.Extensions;
using Cbms.Mediator.Query;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandler
{
    public class TicketGetByTicketInvestmentIdHandler : QueryHandlerBase, IRequestHandler<TicketGetListByTicketInvestmentId, PagingResult<TicketListDto>>
    {
        private readonly AppDbContext _dbContext;

        public TicketGetByTicketInvestmentIdHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<TicketListDto>> Handle(TicketGetListByTicketInvestmentId request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from tickets in _dbContext.Tickets
                        join staffs in _dbContext.Staffs on tickets.LastPrintUserId equals staffs.Id into staffsL
                        from staffs in staffsL.DefaultIfEmpty()
                        where tickets.TicketInvestmentId == request.TicketInvestmentId
                        select new TicketListDto()
                        {
                            Id = tickets.Id,
                            IsSelected = false,
                            Code = tickets.Code,
                            ConsumerPhone = tickets.ConsumerPhone,
                            ConsumerName = tickets.ConsumerName,
                            IssueDate = tickets.IssueDate,
                            PrintDate = tickets.PrintDate,
                            PrintCount = tickets.PrintCount,
                            LastPrintUserId = tickets.LastPrintUserId,
                            TicketInvestmentId = tickets.TicketInvestmentId,
                            LastPrintUserName = staffs.Name != null ? staffs.Name : tickets.LastPrintUserId.ToString(),
                        };

            query = query
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.ConsumerPhone.Contains(keyword) || x.ConsumerName.Contains(keyword))
                .Where(x => x.TicketInvestmentId == request.TicketInvestmentId);

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
            return new PagingResult<TicketListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }        
    }
}

