using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Kms.Application.ProductUnits.Query;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TenantServers.QueryHandlers
{
    public class GetProductUnitListHandler : QueryHandlerBase, IRequestHandler<GetProductUnitList, PagingResult<ProductUnitDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepository<Product, int> _productRepository;
        public GetProductUnitListHandler(IRequestSupplement supplement,IRepository<Product, int> productRepository, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        public async Task<PagingResult<ProductUnitDto>> Handle(GetProductUnitList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            int productId = request.ProductId;

            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == productId);

            var query = from p in _dbContext.ProductUnits
                        where p.Code == product.Unit || p.Code == product.CaseUnit
                        select new ProductUnitDto()
                        {
                            Code = p.Code,
                            Name = p.Name,
                            IsActive = p.IsActive,
                            Id = p.Id
                        };


            int totalCount = query.Count();

            query = query.SortFromString(request.Sort);

            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<ProductUnitDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}
