using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerGetHandler : QueryHandlerBase, IRequestHandler<CustomerGet, CustomerDto>
    {
        private readonly IRepository<Customer, int> _repository;

        public CustomerGetHandler(IRequestSupplement supplement, IRepository<Customer, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<CustomerDto> Handle(CustomerGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<CustomerDto>(await _repository.GetAsync(request.Id));
        }
    }
}