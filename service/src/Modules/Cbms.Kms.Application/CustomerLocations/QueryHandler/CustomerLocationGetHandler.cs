using Cbms.Domain.Repositories;
using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Kms.Application.CustomerLocations.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.CustomerLocations.QueryHandler
{
    public class CustomerLocationGetHandler : QueryHandlerBase, IRequestHandler<CustomerLocationGet, CustomerLocationDto>
    {
        private readonly IRepository<CustomerLocation, int> _repository;

        public CustomerLocationGetHandler(IRequestSupplement supplement, IRepository<CustomerLocation, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<CustomerLocationDto> Handle(CustomerLocationGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<CustomerLocationDto>(await _repository.GetAsync(request.Id));
        }
    }
}