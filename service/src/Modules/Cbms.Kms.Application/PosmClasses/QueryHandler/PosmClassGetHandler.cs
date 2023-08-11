using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Kms.Application.PosmClasses.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmClasses.QueryHandler
{
    public class PosmClassGetHandler : QueryHandlerBase, IRequestHandler<PosmClassGet, PosmClassDto>
    {
        private readonly IRepository<PosmClass, int> _repository;

        public PosmClassGetHandler(IRequestSupplement supplement, IRepository<PosmClass, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<PosmClassDto> Handle(PosmClassGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<PosmClassDto>(await _repository.GetAsync(request.Id));
        }
    }
}