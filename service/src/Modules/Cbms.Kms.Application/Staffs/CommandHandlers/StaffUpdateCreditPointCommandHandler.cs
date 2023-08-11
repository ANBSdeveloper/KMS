using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Staffs.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Cbms.Application.Staffs.Commands;

namespace Cbms.Kms.Application.Staffs.CommandHandlers
{
    public class StaffUpdateCreditPointCommandHandler : RequestHandlerBase, IRequestHandler<StaffUpdateCreditPointCommand>
    {
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public StaffUpdateCreditPointCommandHandler(
            DistributedLockManager distributedLockManager, 
            IRequestSupplement supplement, 
            IRepository<Staff, int> staffRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _staffRepository = staffRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(StaffUpdateCreditPointCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            await using (await _distributedLockManager.AcquireAsync($"update_credit_point" + requestData.StaffId))
            {
                var staff = await _staffRepository.GetAsync(requestData.StaffId);

            
                await staff.ApplyActionAsync(new StaffUpdateCreditPointAction(requestData.CreditPoint));

                await _staffRepository.UnitOfWork.CommitAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}