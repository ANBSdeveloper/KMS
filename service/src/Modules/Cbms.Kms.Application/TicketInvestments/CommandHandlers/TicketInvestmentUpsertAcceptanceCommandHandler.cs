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
    public class TicketInvestmentUpsertAcceptanceCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpsertAcceptanceCommand, TicketInvestmentDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentUpsertAcceptanceCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentUpsertAcceptanceCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"ticket_investment_" + requestData.Id))
            {
                var ticketInvestment = await _ticketInvestmentRepository
                    .GetAllIncluding(p => p.TicketOperation, p => p.TicketAcceptance, p => p.RewardItems, p => p.ConsumerRewards)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

                if (ticketInvestment == null)
                {
                    throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
                }

                await ticketInvestment.ApplyActionAsync(new TicketInvestmentAcceptAction(
                    IocResolver,
                    LocalizationSource,
                    Session.UserId.Value,
                    requestData.AcceptanceDate,
                    requestData.ActualSalesAmount,
                    requestData.Photo1,
                    requestData.Photo2,
                    requestData.Photo3,
                    requestData.Photo4,
                    requestData.Photo5,
                    requestData.Note,
                    (request.HandleType ?? "").Equals("complete", System.StringComparison.OrdinalIgnoreCase)
                ));

                await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                return await Mediator.Send(new TicketInvestmentGet(ticketInvestment.Id));
            }
        }
    }
}