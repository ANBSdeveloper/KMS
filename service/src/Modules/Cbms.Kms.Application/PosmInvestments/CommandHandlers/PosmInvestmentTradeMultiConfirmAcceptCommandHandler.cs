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
    public class PosmInvestmentTradeMultiConfirmAcceptCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentTradeMultiConfirmAcceptCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentTradeMultiConfirmAcceptCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentTradeMultiConfirmAcceptCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            foreach (var id in request.Data.Ids)
            {
                var posmInvestment = await _posmInvestmentRepository
               .GetAllIncluding(p => p.Items)
               .FirstOrDefaultAsync(p => p.Id == id);

                await using (await _distributedLockManager.AcquireAsync($"posm_investment_" + id))
                {
                    foreach (var item in posmInvestment.Items)
                    {
                        await posmInvestment.ApplyActionAsync(new PosmInvestmentTradeConfirmAcceptAction(
                           IocResolver,
                           LocalizationSource,
                           item.Id));
                    }
                   

                    await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);
                }

            }

            return Unit.Value;
            
        }
    }
}