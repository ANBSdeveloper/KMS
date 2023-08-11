using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.CommandHandlers
{
    public class PosmInvestmentSupplyDenyRequestCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentSupplyDenyRequestCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentSupplyDenyRequestCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentSupplyDenyRequestCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"posm_investment_" + requestData.Id))
            {

                var posmInvestment = await _posmInvestmentRepository
                    .GetAllIncluding(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

                if (posmInvestment == null)
                {
                    throw new EntityNotFoundException(typeof(TicketInvestment), requestData.Id);
                }

                await posmInvestment.ApplyActionAsync(new PosmInvestmentSupplyDenyRequestAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.PosmInvestmentItemId,
                    DateTime.Now,
                    requestData.VendorId,
                    requestData.Note,
                    requestData.ActualUnitPrice
                ));


                await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);
            }

            return Unit.Value;

        }
    }
}