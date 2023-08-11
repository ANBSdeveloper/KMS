using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Kms.Domain.Staffs;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.CommandHandlers
{
    public class CustomerRegisterKeyShopCommandHandler : RequestHandlerBase, IRequestHandler<CustomerRegisterKeyShopCommand>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public CustomerRegisterKeyShopCommandHandler(
            DistributedLockManager distributedLockManager, 
            IRequestSupplement supplement, 
            IRepository<Customer, int> customerRepository,
            IRepository<Staff, int> staffRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _staffRepository = staffRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(CustomerRegisterKeyShopCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            var staff = await _staffRepository.GetAll().FirstOrDefaultAsync(p => p.UserId == Session.UserId);
            if (staff == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Staff.NotExistsWithUser", Session.UserName).Build();
            }
            foreach (var customerId in requestData)
            {
                await using (await _distributedLockManager.AcquireAsync($"register_key_shop_id_" + customerId.ToString()))
                {
                    var customer = await _customerRepository.GetAsync(customerId);
                    await customer.ApplyActionAsync(new CustomerRegisterKeyShopAction(LocalizationSource, staff.Id));
                }
            }
            await _customerRepository.UnitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}