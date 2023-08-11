using Cbms.Domain.Repositories;
using Cbms.Kms.Application.MaterialTypes.Dto;
using Cbms.Kms.Application.MaterialTypes.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.MaterialTypes;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.MaterialTypes.QueryHandler
{
    public class CustomerLocationGetHandler : QueryHandlerBase, IRequestHandler<MaterialTypeGet, MaterialTypeDto>
    {
        private readonly IRepository<MaterialType, int> _repository;

        public CustomerLocationGetHandler(IRequestSupplement supplement, IRepository<MaterialType, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<MaterialTypeDto> Handle(MaterialTypeGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<MaterialTypeDto>(await _repository.GetAsync(request.Id));
        }
    }
}