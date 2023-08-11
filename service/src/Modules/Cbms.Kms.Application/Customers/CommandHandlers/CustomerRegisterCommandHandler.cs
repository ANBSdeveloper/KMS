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
    public class CustomerRegisterCommandHandler : RequestHandlerBase, IRequestHandler<CustomerRegisterCommand>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public CustomerRegisterCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Customer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"shop_register" + request.Data.UserName))
            {
                var customer = new Customer();
                await customer.ApplyActionAsync(new CustomerRegisterAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.UserName,
                    requestData.FullName,
                    requestData.Phone,
                    requestData.Password
                ));

                await _customerRepository.InsertAsync(customer);
                return Unit.Value;
            }
        }
    }
}