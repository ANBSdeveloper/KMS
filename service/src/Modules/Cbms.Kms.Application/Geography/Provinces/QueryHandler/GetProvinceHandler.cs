using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Geography.Provinces.Dto;
using Cbms.Kms.Application.Geography.Provinces.Query;
using Cbms.Kms.Domain.Geography.Provinces;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Provinces.QueryHandlers
{
    public class GetProvinceHandler : QueryHandlerBase, IRequestHandler<GetProvince, ProvinceDto>
    {
        private readonly IRepository<Province, int> _repository;

        public GetProvinceHandler(IRequestSupplement supplement, IRepository<Province, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<ProvinceDto> Handle(GetProvince request, CancellationToken cancellationToken)
        {
            return Mapper.Map<ProvinceDto>(await _repository.GetAsync(request.Id));
        }
    }
}