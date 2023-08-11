using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Kms.Application.ProductUnits.Query;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TenantServers.QueryHandlers
{
    public class GetProductUnitHandler : QueryHandlerBase, IRequestHandler<GetProductUnit, ProductUnitDto>
    {
        private readonly IRepository<ProductUnit, int> _repository;

        public GetProductUnitHandler(IRequestSupplement supplement, IRepository<ProductUnit, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<ProductUnitDto> Handle(GetProductUnit request, CancellationToken cancellationToken)
        {
            return Mapper.Map<ProductUnitDto>(await _repository.GetAsync(request.Id));
        }
    }
}