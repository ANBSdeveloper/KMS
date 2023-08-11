using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.CommandHandlers
{
    public class PosmInvestmentSupplyConfirmRequestCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentSupplyConfirmRequestCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentSupplyConfirmRequestCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentSupplyConfirmRequestCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"posm_investment_"+ requestData.Id))
            {

                var posmInvestment = await _posmInvestmentRepository
                    .GetAllIncluding(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

                if (posmInvestment == null)
                {
                    throw new EntityNotFoundException(typeof(PosmInvestment), requestData.Id);
                }

                await posmInvestment.ApplyActionAsync(new PosmInvestmentSupplyConfirmRequestAction(
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