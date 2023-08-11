using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Geography.Districts.Dto;
using Cbms.Kms.Application.Geography.Districts.Query;
using Cbms.Kms.Domain.Geography.Districts;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Districts.QueryHandlers
{
    public class GetDistrictHandler : QueryHandlerBase, IRequestHandler<GetDistrict, DistrictDto>
    {
        private readonly IRepository<District, int> _repository;

        public GetDistrictHandler(IRequestSupplement supplement, IRepository<District, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<DistrictDto> Handle(GetDistrict request, CancellationToken cancellationToken)
        {
            return Mapper.Map<DistrictDto>(await _repository.GetAsync(request.Id));
        }
    }
}