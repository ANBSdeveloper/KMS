using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Kms.Application.ProductPoints.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductPoints.QueryHandlers
{
    public class ProductPointGetListHandler : QueryHandlerBase, IRequestHandler<ProductPointGetList, PagingResult<ProductPointListItemDto>>
    {
        private readonly AppDbContext _dbContext;

        public ProductPointGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<ProductPointListItemDto>> Handle(ProductPointGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from point in _dbContext.ProductPoints
                        join product in _dbContext.Products on point.ProductId equals product.Id
                        join brand in _dbContext.Brands on product.BrandId equals brand.Id
                        join productClass in _dbContext.ProductClasses on product.ProductClassId equals productClass.Id
                        join subProductClass in _dbContext.SubProductClasses on product.SubProductClassId equals subProductClass.Id
                        select new ProductPointListItemDto()
                        {
                            CreationTime = point.CreationTime,
                            CreatorUserId = point.CreatorUserId,
                            FromDate = point.FromDate,
                            Id = point.Id,
                            IsActive = point.IsActive,
                            LastModificationTime = point.LastModificationTime,
                            LastModifierUserId = point.LastModifierUserId,
                            Points = point.Points,
                            ProductClassId = productClass != null ? productClass.Id : default(int?),
                            ProductClassName = productClass.Name,
                            ProductCode = product.Code,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            SubProductClassName = subProductClass.Name,
                            ToDate = point.ToDate,
                            Unit = product.Unit,
                        };

            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(request.ProductClassId.HasValue, p => p.ProductClassId == request.ProductClassId)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.ProductCode.Contains(keyword) ||
                    x.ProductName.Contains(keyword) || x.ProductClassName.Contains(keyword) || x.SubProductClassName.Contains(keyword));

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
            return new PagingResult<ProductPointListItemDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}