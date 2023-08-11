using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketInvestmentHistoryGetHandler : QueryHandlerBase, IRequestHandler<TicketInvestmentHistoryGet, List<TicketInvestmentHistoryDto>>
    {
        private readonly AppDbContext _dbContext;

        public TicketInvestmentHistoryGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<List<TicketInvestmentHistoryDto>> Handle(TicketInvestmentHistoryGet request, CancellationToken cancellationToken)
        {
            var entityDtos = await (from p in _dbContext.TicketInvestmentHistories
                                    join u in _dbContext.Users on p.CreatorUserId equals u.Id
                                    where p.TicketInvestmentId == request.Id
                                    select new TicketInvestmentHistoryDto()
                                    {
                                        Id = p.Id,
                                        CreationTime = p.CreationTime,
                                        Data = p.Data,
                                        Status = (int)p.Status,
                                        UserId = p.CreatorUserId.Value,
                                        UserName = u.Name,
                                        UserCode = u.UserName,
                                    }).ToListAsync();

            return entityDtos;
        }
    }
}