using Cbms.Application.Runtime.DistributedLock;
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
    public class PosmInvestmentMarketingConfirmProduceCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentMarketingConfirmProduceCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentMarketingConfirmProduceCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentMarketingConfirmProduceCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var posmInvestment = await _posmInvestmentRepository
                    .GetAllIncluding(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == requestData.Id);

            await using (await _distributedLockManager.AcquireAsync($"posm_investment_"+ requestData.Id))
            {
                await posmInvestment.ApplyActionAsync(new PosmInvestmentMarketingConfirmProduceAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.PosmInvestmentItemId,
                    requestData.Photo1,
                    requestData.Photo2,
                    requestData.Photo3,
                    requestData.Photo4,
                    requestData.Link,
                    requestData.Note));


                await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);
            }

            return Unit.Value;
            
        }
    }
}