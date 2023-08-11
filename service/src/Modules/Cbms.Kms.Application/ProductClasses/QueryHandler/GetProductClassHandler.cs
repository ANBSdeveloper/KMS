using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Kms.Application.ProductClasses.Query;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TenantServers.QueryHandlers
{
    public class GetProductClassHandler : QueryHandlerBase, IRequestHandler<GetProductClass, ProductClassDto>
    {
        private readonly IRepository<ProductClass, int> _repository;

        public GetProductClassHandler(IRequestSupplement supplement, IRepository<ProductClass, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<ProductClassDto> Handle(GetProductClass request, CancellationToken cancellationToken)
        {
            return Mapper.Map<ProductClassDto>(await _repository.GetAsync(request.Id));
        }
    }
}