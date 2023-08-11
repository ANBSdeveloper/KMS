using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketInvestmentSummaryGetHandler : QueryHandlerBase, IRequestHandler<TicketInvestmentSummaryGet, TicketInvestmentSummaryDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly ICustomerManager _customerManager;

        public TicketInvestmentSummaryGetHandler(
            IRequestSupplement supplement,
            IRepository<TicketInvestment, int> ticketInvestmentRepository,
            ICustomerManager customerManager) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _customerManager = customerManager;
        }

        public async Task<TicketInvestmentSummaryDto> Handle(TicketInvestmentSummaryGet request, CancellationToken cancellationToken)
        {
            var ticketInvestment = await _ticketInvestmentRepository
                .GetAllIncluding(p => p.TicketAcceptance, prop => prop.Tickets)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (ticketInvestment == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), request.Id);
            }

            decimal actualAmount = 0;
            if (ticketInvestment.Status != TicketInvestmentStatus.Accepted)
            {
                actualAmount = await _customerManager.GetActualSalesAmountAsync(ticketInvestment.CustomerId, ticketInvestment.BuyBeginDate, ticketInvestment.BuyEndDate);
            }

            return new TicketInvestmentSummaryDto()
            {
                Id = ticketInvestment.Id,
                ActualSalesAmount = actualAmount,
                CommitmentSalesAmount = ticketInvestment.CommitmentAmount,
                PrintTicketQuantity = ticketInvestment.PrintTicketQuantity,
                SmsTicketQuantity = ticketInvestment.SmsTicketQuantity,
                TicketQuantity = ticketInvestment.TicketQuantity,
                RemarkOfCompany = ticketInvestment.TicketAcceptance == null ? 0 : ticketInvestment.TicketAcceptance.RemarkOfCompany,
                RemarkOfCustomerDevelopement = ticketInvestment.TicketAcceptance == null ? 0 : ticketInvestment.TicketAcceptance.RemarkOfCustomerDevelopement,
                RemarkOfSales = ticketInvestment.TicketAcceptance == null ? 0 : ticketInvestment.TicketAcceptance.RemarkOfSales
            };
        }
    }
}