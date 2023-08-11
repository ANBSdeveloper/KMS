using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Geography.Zones.Dto;
using Cbms.Kms.Application.Geography.Zones.Query;
using Cbms.Kms.Domain.Zones;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Zones.Queryhandler
{
    public class GetZoneHandler : QueryHandlerBase, IRequestHandler<GetZone, ZoneDto>
    {
        private readonly IRepository<Zone, int> _repository;

        public GetZoneHandler(IRequestSupplement supplement, IRepository<Zone, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<ZoneDto> Handle(GetZone request, CancellationToken cancellationToken)
        {
            return Mapper.Map<ZoneDto>(await _repository.GetAsync(request.Id));
        }
    }
}