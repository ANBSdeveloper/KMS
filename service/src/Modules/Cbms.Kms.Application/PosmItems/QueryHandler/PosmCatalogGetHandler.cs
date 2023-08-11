using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.QueryHandler
{
    public class PosmCatalogGetHandler : QueryHandlerBase, IRequestHandler<PosmCatalogGet, PosmCatalogDto>
    {
        private readonly IRepository<PosmCatalog, int> _repository;

        public PosmCatalogGetHandler(IRequestSupplement supplement, IRepository<PosmCatalog, int> repository) : base(supplement)
        {
            _repository = repository;
        }

        public async Task<PosmCatalogDto> Handle(PosmCatalogGet request, CancellationToken cancellationToken)
        {

            return Mapper.Map<PosmCatalogDto>(await _repository.GetAsync(request.Id));
        }
    }
}