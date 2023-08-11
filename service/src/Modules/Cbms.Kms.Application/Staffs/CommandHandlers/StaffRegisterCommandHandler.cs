using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Staffs.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Staffs.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Staffs.CommandHandlers
{
    public class StaffRegisterCommandHandler : RequestHandlerBase, IRequestHandler<StaffRegisterCommand>
    {
        private readonly IRepository<Staff, int> _customerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public StaffRegisterCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Staff, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(StaffRegisterCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"staff_register" + request.Data.UserName))
            {
                var staff = new Staff();
                await staff.ApplyActionAsync(new StaffRegisterAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.UserName,
                    requestData.FullName,
                    requestData.Phone,
                    requestData.Password
                ));

                await _customerRepository.InsertAsync(staff);
                return Unit.Value;
            }
        }
    }
}