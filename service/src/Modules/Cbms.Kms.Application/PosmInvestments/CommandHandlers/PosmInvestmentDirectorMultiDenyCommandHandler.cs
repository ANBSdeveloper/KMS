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
    public class PosmInvestmentDirectorMultiDenyCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentDirectorMultiDenyCommand>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentDirectorMultiDenyCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> posmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = posmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(PosmInvestmentDirectorMultiDenyCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            foreach (var id in request.Data.Ids)
            {
                var posmInvestment = await _posmInvestmentRepository
                .GetAllIncluding(p => p.Items)
                 .FirstOrDefaultAsync(p => p.Id == id);

                    await using (await _distributedLockManager.AcquireAsync($"posm_investment_" + id))
                    {
                        await posmInvestment.ApplyActionAsync(new PosmInvestmentDirectorDenyRequestAction(
                            IocResolver,
                            LocalizationSource,
                            requestData.Note));

                        await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);
                    }

            }


            return Unit.Value;
            
        }
    }
}