using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    public class TicketInvestmentCustomerDevelopmentRemarkCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentCustomerDevelopmentRemarkCommand>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentCustomerDevelopmentRemarkCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(TicketInvestmentCustomerDevelopmentRemarkCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"ticket_investment_" + requestData.Id))
            {
                var ticketInvestment = await _ticketInvestmentRepository.GetAllIncluding(p => p.TicketAcceptance)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

                if (ticketInvestment == null)
                {
                    throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
                }

                await ticketInvestment.ApplyActionAsync(new TicketAcceptanceCustomerDevelopmentRemarkAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.Remark
                ));

                await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}