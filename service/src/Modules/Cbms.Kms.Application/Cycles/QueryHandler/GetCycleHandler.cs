using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Kms.Application.Cycles.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Cycles;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Cycles.QueryHandlers
{
    public class GetCycleHandler : QueryHandlerBase, IRequestHandler<GetCycle, CycleDto>
    {
        private readonly IRepository<Cycle, int> _repository;

        public GetCycleHandler(IRequestSupplement supplement, IRepository<Cycle, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<CycleDto> Handle(GetCycle request, CancellationToken cancellationToken)
        {
            return Mapper.Map<CycleDto>(await _repository.GetAsync(request.Id));
        }
    }
}