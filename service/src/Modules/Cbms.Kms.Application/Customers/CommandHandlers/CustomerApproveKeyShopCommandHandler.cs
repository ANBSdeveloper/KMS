using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.CommandHandlers
{
    public class CustomerApproveKeyShopCommandHandler : RequestHandlerBase, IRequestHandler<CustomerApproveKeyShopCommand>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public CustomerApproveKeyShopCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Customer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(CustomerApproveKeyShopCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            foreach (var customerId in requestData)
            {
                await using (await _distributedLockManager.AcquireAsync($"approve_key_shop_id_" + customerId.ToString()))
                {
                    var customer = await _customerRepository.GetAsync(customerId);
                    await customer.ApplyActionAsync(new CustomerApproveKeyShopAction(LocalizationSource));
                }
            }
            await _customerRepository.UnitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}