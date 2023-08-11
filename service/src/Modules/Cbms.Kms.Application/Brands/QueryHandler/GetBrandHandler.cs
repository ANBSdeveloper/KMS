using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Brands.Dto;
using Cbms.Kms.Application.Brands.Query;
using Cbms.Kms.Domain.Brands;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Brands.QueryHandlers
{
    public class GetBrandHandler : QueryHandlerBase, IRequestHandler<GetBrand, BrandDto>
    {
        private readonly IRepository<Brand, int> _repository;

        public GetBrandHandler(IRequestSupplement supplement, IRepository<Brand, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<BrandDto> Handle(GetBrand request, CancellationToken cancellationToken)
        {
            return Mapper.Map<BrandDto>(await _repository.GetAsync(request.Id));
        }
    }
}