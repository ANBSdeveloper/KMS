using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Products.QueryHandlers
{
    public class GetProductListHandler : QueryHandlerBase, IRequestHandler<GetProductList, PagingResult<ProductListItemDto>>
    {
        private readonly AppDbContext _dbContext;
        public GetProductListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<ProductListItemDto>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from product in _dbContext.Products
                        join brand in _dbContext.Brands on product.BrandId equals brand.Id
                        join productClass in _dbContext.ProductClasses on product.ProductClassId equals productClass.Id
                        join subProductClass in _dbContext.SubProductClasses on product.SubProductClassId equals subProductClass.Id
                        select new ProductListItemDto()
                        {
                            BrandId = product.BrandId,
                            BrandName = brand.Name,
                            CaseUnit = product.CaseUnit,
                            Code = product.Code,
                            CreationTime = product.CreationTime,
                            CreatorUserId = product.CreatorUserId,
                            Id = product.Id,
                            IsActive = product.IsActive,
                            LastModificationTime = product.LastModificationTime,
                            LastModifierUserId = product.LastModifierUserId,
                            Name = product.Name,
                            PackSize = product.PackSize,
                            ProductClassId = product.ProductClassId,
                            ProductClassCode = productClass.Code,
                            ProductClassName = productClass.Name,
                            SubProductClassId = product.SubProductClassId,
                            SubProductClassName = subProductClass.Name,
                            Unit = product.Unit,
                            UpdateDate = product.UpdateDate,
                        };

            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                   EF.Functions.Like(x.Name, $"{keyword}%") || EF.Functions.Like(x.BrandName, $"{keyword}%") || EF.Functions.Like(x.ProductClassName, $"{keyword}%"))
                 .WhereIf(!string.IsNullOrEmpty(request.ProductClassCode), x => x.ProductClassCode == request.ProductClassCode);

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
            return new PagingResult<ProductListItemDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}