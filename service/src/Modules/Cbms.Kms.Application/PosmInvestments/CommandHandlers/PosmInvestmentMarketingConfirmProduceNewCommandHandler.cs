﻿using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.CommandHandlers
{
    public class PosmInvestmentMarketingConfirmProduceNewCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentMarketingConfirmProduceNewCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentMarketingConfirmProduceNewCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentMarketingConfirmProduceNewCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var posmInvestment = await _posmInvestmentRepository
                    .GetAllIncluding(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

            await using (await _distributedLockManager.AcquireAsync($"posm_investment_"+ requestData.Id))
            {
                await posmInvestment.ApplyActionAsync(new PosmInvestmentMarketingConfirmProduceNewAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.PosmInvestmentId,
                    requestData.DesignPhoto1,
                    requestData.DesignPhoto2,
                    requestData.DesignPhoto3,
                    requestData.DesignPhoto4,
                    requestData.Link,
                    requestData.Note));


                await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);
            }

            return Unit.Value;
            
        }
    }
}