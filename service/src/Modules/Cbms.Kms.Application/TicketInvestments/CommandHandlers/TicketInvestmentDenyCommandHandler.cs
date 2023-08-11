using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
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
    public class TicketInvestmentDenyCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentDenyCommand>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentDenyCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(TicketInvestmentDenyCommand request, CancellationToken cancellationToken)
        {
            await using (await _distributedLockManager.AcquireAsync($"ticket_investment_" + request.Id))
            {
                var investment = await _ticketInvestmentRepository.GetAsync(request.Id);

                if (investment == null)
                {
                    throw new EntityNotFoundException(typeof(TicketInvestment), request.Id);
                }

                await investment.ApplyActionAsync(new TicketInvestmentDenyAction(
                    IocResolver,
                    LocalizationSource,
                    Session.UserId.Value));
                 
                await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}