using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Domain.Products;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Products.QueryHandlers
{
    public class GetProductHandler : QueryHandlerBase, IRequestHandler<GetProduct, ProductBaseDto>
    {
        private readonly IRepository<Product, int> _repository;
        public GetProductHandler(IRequestSupplement supplement, IRepository<Product, int> repository) : base(supplement)
        {
            _repository = repository;
        }

        public async Task<ProductBaseDto> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            return Mapper.Map<ProductBaseDto>(await _repository.GetAsync(request.Id));
        }
    }
}