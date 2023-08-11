using Cbms.Domain.Entities;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketInvestmentActiveGetHandler : QueryHandlerBase, IRequestHandler<TicketInvestmentActiveGet, TicketInvestmentDto>
    {
        private readonly AppDbContext _dbContext;

        public TicketInvestmentActiveGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentActiveGet request, CancellationToken cancellationToken)
        {
            var customer = _dbContext.Customers.FirstOrDefault(p => p.UserId == Session.UserId);
            if (customer == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.UserIsNotCustomer").Build();
            }

            var ticketInvestment = await _dbContext.TicketInvestments
                .Where(p => p.CustomerId == customer.Id && p.IsActive)
                .OrderByDescending(p=>p.CreationTime)
                .FirstOrDefaultAsync();

            if (ticketInvestment == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("TicketInvestment.CustomerNoHaveActive").Build();
            }

            return await Mediator.Send(new TicketInvestmentGet(ticketInvestment.Id));
        }
    }
}