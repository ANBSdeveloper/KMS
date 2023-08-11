using Cbms.Domain.Entities;
using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Kms.Application.ProductPoints.Query;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductPoints.QueryHandlers
{
    public class ProductPointGetHandler : QueryHandlerBase, IRequestHandler<ProductPointGet, ProductPointDto>
    {
        private readonly AppDbContext _dbContext;

        public ProductPointGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductPointDto> Handle(ProductPointGet request, CancellationToken cancellationToken)
        {
            var entitDto = await (from point in _dbContext.ProductPoints
                                  join product in _dbContext.Products on point.ProductId equals product.Id
                                  join brand in _dbContext.Brands on product.BrandId equals brand.Id
                                  join productClass in _dbContext.ProductClasses on product.ProductClassId equals productClass.Id
                                  join subProductClass in _dbContext.SubProductClasses on product.SubProductClassId equals subProductClass.Id
                                  where point.Id == request.Id
                                  select new ProductPointDto()
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
                                      Unit = product.Unit
                                  }).FirstOrDefaultAsync();
            if (entitDto == null)
            {
                throw new EntityNotFoundException(typeof(ProductPoint), request.Id);
            }
            return entitDto;
        }
    }
}