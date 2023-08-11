

using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandler
{
    public class TicketGetByConsumerHandler : QueryHandlerBase, IRequestHandler<TicketGetByConsumer, List<TicketGetByConsumerDto>>
    {
        private readonly AppDbContext _dbContext;

        public TicketGetByConsumerHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TicketGetByConsumerDto>> Handle(TicketGetByConsumer request, CancellationToken cancellationToken)
        {
     
            var query = from ticket in _dbContext.Tickets
                        join investment in _dbContext.TicketInvestments on ticket.TicketInvestmentId equals investment.Id
                        join customer in _dbContext.Customers on investment.CustomerId equals customer.Id
                        where ticket.ConsumerPhone == request.Phone
                        select new TicketGetByConsumerDto()
                        {
                            TicketCode = ticket.Code,
                            EndDate = investment.IssueTicketEndDate,
                            OperationDate = investment.OperationDate,
                            ShopAddress = customer.Address,
                            ShopName = customer.Name,
                            ShopCode = customer.Code
                        };

            return await query.ToListAsync();
        }        
    }
}

