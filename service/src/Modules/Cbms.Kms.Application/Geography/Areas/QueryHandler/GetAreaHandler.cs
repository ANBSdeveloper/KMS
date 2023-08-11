using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Kms.Application.Geography.Areas.Query;
using Cbms.Kms.Domain.Areas;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Geography.Areas.QueryHandler
{
    public class GetAreaHandler : QueryHandlerBase, IRequestHandler<GetArea, AreaDto>
    {
        private readonly IRepository<Area, int> _repository;

        public GetAreaHandler(IRequestSupplement supplement, IRepository<Area, int> repository) : base(supplement)
        {
            LocalizationSourceName = "Stock";
            _repository = repository;
        }

        public async Task<AreaDto> Handle(GetArea request, CancellationToken cancellationToken)
        {
            return Mapper.Map<AreaDto>(await _repository.GetAsync(request.Id));
        }
    }
}