using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Helpers;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    public class TicketInvestmentUpsertProgressCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpsertProgressCommand, TicketInvestmentDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;

        public TicketInvestmentUpsertProgressCommandHandler(IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentUpsertProgressCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var ticketInvestment = await _ticketInvestmentRepository.GetAllIncluding(p=>p.Materials, p=> p.RewardItems, p => p.Progresses)
                    .FirstOrDefaultAsync(p=>p.Id == requestData.Id);

            if (ticketInvestment == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
            }


            await ticketInvestment.ApplyActionAsync(new TicketProgressUpsertAction(
                IocResolver,
                LocalizationSource,
                Session.UserId.Value,
                requestData.ProgressId,
                requestData.DocumentPhoto1,
                requestData.DocumentPhoto2,
                requestData.DocumentPhoto3,
                requestData.DocumentPhoto4,
                requestData.DocumentPhoto5,
                requestData.Note,
                requestData.UpsertRewardItems.Select(p => new TicketProgressUpsertAction.RewardItem(
                    p.Id,
                    p.RewardItemId,
                    p.IsReceived)).ToList(),
                requestData.UpsertMaterials.Select(p => new TicketProgressUpsertAction.Material(
                    p.Id,
                    p.MaterialId,
                    p.IsReceived,
                    p.IsSentDesign,
                    p.Photo1,
                    p.Photo2,
                    p.Photo3,
                    p.Photo4,
                    p.Photo5)).ToList(), 
                    ticketInvestment.RewardItems.ToList(),
                    ticketInvestment.Materials.ToList()));

            await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await Mediator.Send(new TicketInvestmentGet(ticketInvestment.Id));
        }
    }
}