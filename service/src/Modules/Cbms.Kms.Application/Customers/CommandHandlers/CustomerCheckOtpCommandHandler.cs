using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.CommandHandlers
{
    public class CustomerCheckOtpCommandHandler : RequestHandlerBase, IRequestHandler<CustomerCheckOtpCommand>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public CustomerCheckOtpCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Customer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(CustomerCheckOtpCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"customer_check_otp_" + request.Data.MobilePhone))
            {
                var customer = _customerRepository.GetAll().FirstOrDefault(p => p.MobilePhone == request.Data.MobilePhone);

                if (customer == null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.NotExistsWithPhone", request.Data.MobilePhone).Build();
                }

                await customer.ApplyActionAsync(new CustomerCheckOtpAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.OtpCode
                ));

                return Unit.Value;
            }
        }
    }
}