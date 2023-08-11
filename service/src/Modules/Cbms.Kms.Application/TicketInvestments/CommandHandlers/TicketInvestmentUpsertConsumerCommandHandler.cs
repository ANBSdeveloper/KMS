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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    public class TicketInvestmentUpsertConsumerCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpsertConsumerRewardCommand, TicketConsumerRewardDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;

        public TicketInvestmentUpsertConsumerCommandHandler(IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
        }

        public async Task<TicketConsumerRewardDto> Handle(TicketInvestmentUpsertConsumerRewardCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var ticketInvestment = await _ticketInvestmentRepository.GetAll()
                .Include(p => p.ConsumerRewards).ThenInclude(p => p.Details)
                .Include(p => p.RewardItems)
                .Include(p => p.Tickets).FirstOrDefaultAsync(p => p.Id == requestData.Id);

            if (ticketInvestment == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
            }

            await ticketInvestment.ApplyActionAsync(new TicketConsumerRewardUpsertAction(
                IocResolver,
                LocalizationSource,
                Session.UserId.Value,
                requestData.RewardItemId,
                requestData.Quantity,
                requestData.Photo1,
                requestData.Photo2,
                requestData.Photo3,
                requestData.Photo4,
                requestData.Photo5,
                requestData.DetailChanges.UpsertedItems.Select(p => new TicketConsumerRewardUpsertAction.ConsumerRewardDetail(
                    p.Id,
                    p.TicketId,
                    p.Note)).ToList(),
                requestData.DetailChanges.DeletedItems.Select(p => p.Id).ToList(),
                ticketInvestment.Tickets.ToList()));

            await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

            var consumerReward = ticketInvestment.ConsumerRewards.FirstOrDefault(p => p.RewardItemId == requestData.RewardItemId);
            return await Mediator.Send(new TicketConsumerRewardGet(consumerReward.Id));
        }
    }
}