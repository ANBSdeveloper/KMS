using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    public class TicketInvestmentUpdateCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpdateCommand>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;

        public TicketInvestmentUpdateCommandHandler(
            IRequestSupplement supplement, 
            IRepository<TicketInvestment, int> ticketInvestmentRepository
        ) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
        }

        public async Task<Unit> Handle(TicketInvestmentUpdateCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            var ticketInvestment = await _ticketInvestmentRepository.GetAsync(request.Data.Id);
            await ticketInvestment.ApplyActionAsync(new TicketInvestmentUpdateAction(IocResolver, LocalizationSource, requestData.OperationDate));
            return Unit.Value;
        }
    }
}