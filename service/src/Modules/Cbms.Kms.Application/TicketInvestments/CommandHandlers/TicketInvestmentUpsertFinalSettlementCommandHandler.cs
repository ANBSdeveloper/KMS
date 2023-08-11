using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
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
    public class TicketInvestmentUpsertFinalSettlementCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpsertFinalSettlementCommand, TicketInvestmentDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentUpsertFinalSettlementCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentUpsertFinalSettlementCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"ticket_investment_" + requestData.Id))
            {
                var ticketInvestment = await _ticketInvestmentRepository
                    .GetAllIncluding(p => p.TicketFinalSettlement, p => p.TicketAcceptance)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

                if (ticketInvestment == null)
                {
                    throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
                }

                await ticketInvestment.ApplyActionAsync(new TicketFinalSettlementUpsertAction(
                    IocResolver,
                    LocalizationSource,
                    Session.UserId.Value,
                    requestData.Note,
                    requestData.Date,
                    requestData.DecideUserId,
                    (request.HandleType ?? "").Equals("complete", System.StringComparison.OrdinalIgnoreCase)
                ));

                await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                return await Mediator.Send(new TicketInvestmentGet(ticketInvestment.Id));
            }
        }
    }
}