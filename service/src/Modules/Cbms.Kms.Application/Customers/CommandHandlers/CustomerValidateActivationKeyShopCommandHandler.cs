﻿using Cbms.Application.Runtime.DistributedLock;
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
    public class CustomerValidateActivationKeyShopCommandHandler : RequestHandlerBase, IRequestHandler<CustomerValidateActivationKeyShopCommand>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public CustomerValidateActivationKeyShopCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Customer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(CustomerValidateActivationKeyShopCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"validate_key_shop_" + request.Data.Code))
            {
                var customer = _customerRepository.GetAll().FirstOrDefault(p => p.Code == request.Data.Code);

                if (customer == null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.NotExists", request.Data.Code).Build();
                }

                await customer.ApplyActionAsync(new CustomerValidateActivationKeyShopAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.AuthCode,
                    requestData.MobilePhone,
                    requestData.Name,
                    requestData.Birthday
                ));

                return Unit.Value;
            }
        }
    }
}