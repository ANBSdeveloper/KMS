using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerGetByCodeHandler : QueryHandlerBase, IRequestHandler<CustomerGetByCode, CustomerDto>
    {
        private readonly IRepository<Customer, int> _repository;

        public CustomerGetByCodeHandler(IRequestSupplement supplement, IRepository<Customer, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<CustomerDto> Handle(CustomerGetByCode request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetAll().FirstOrDefaultAsync(p => p.Code == request.Code);
            if (customer == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.NotExists", request.Code).Build();
            }
            return Mapper.Map<CustomerDto>(customer);
        }
    }
}