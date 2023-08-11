using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    class TicketInvestmentUpdatePrintTicketQuantityCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentUpdatePrintTicketQuantityCommand>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentUpdatePrintTicketQuantityCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(TicketInvestmentUpdatePrintTicketQuantityCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Id;
 
            var ticketInvestment = await _ticketInvestmentRepository.GetAsync(requestData);
            if (ticketInvestment == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("TicketInvestment.IdNotFind", requestData.ToString()).Build();
            }
            else
            {
                if(ticketInvestment.Status !=TicketInvestmentStatus.Approved && ticketInvestment.Status != TicketInvestmentStatus.Doing && ticketInvestment.Status != TicketInvestmentStatus.Operated)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("TicketInvestment.NotPrinted").Build();
                }
            }
            await ticketInvestment.ApplyActionAsync(new TicketInvesmentUpsertPrintTicketQuantityAction(
                IocResolver,
                LocalizationSource, 
                Session.UserId, 
                request.Data));

            await _ticketInvestmentRepository.UnitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
