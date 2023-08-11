using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Kms.Application.PosmTypes.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmTypes;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmTypes.QueryHandler
{
    public class PosmTypeGetHandler : QueryHandlerBase, IRequestHandler<PosmTypeGet, PosmTypeDto>
    {
        private readonly IRepository<PosmType, int> _repository;

        public PosmTypeGetHandler(IRequestSupplement supplement, IRepository<PosmType, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<PosmTypeDto> Handle(PosmTypeGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<PosmTypeDto>(await _repository.GetAsync(request.Id));
        }
    }
}