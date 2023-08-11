using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Localization.Sources;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Cbms.Kms.Application.Products.QueryHandlers
{
    public class GetProductItemHandler : QueryHandlerBase, IRequestHandler<GetProductItem, ProductItemDto>
    {
        private readonly AppDbContext _dbContext;

        public GetProductItemHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<ProductItemDto> Handle(GetProductItem request, CancellationToken cancellationToken)
        {
            string strActive = LocalizationSource.GetString("Product.Active");
            string strNotActive = LocalizationSource.GetString("Product.NotActive");
            var entitDto = await (from product in _dbContext.Products
                                  join brand in _dbContext.Brands on product.BrandId equals brand.Id
                                  join productClass in _dbContext.ProductClasses on product.ProductClassId equals productClass.Id
                                  join subProductClass in _dbContext.SubProductClasses on product.SubProductClassId equals subProductClass.Id
                                  where product.Id == request.Id
                                  select new ProductItemDto()
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
                                      ProductClassName = productClass.Name,
                                      SubProductClassId = product.SubProductClassId,
                                      SubProductClassName = subProductClass.Name,
                                      Unit = product.Unit,
                                      UpdateDate = product.UpdateDate,
                                      Status = product.IsActive == true ? strActive : strNotActive,
                                      Description = product.Description
                                  }).FirstOrDefaultAsync();
            if (entitDto == null)
            {
                throw new EntityNotFoundException(typeof(ProductItemDto), request.Id);
            }
            return entitDto;
        }
    }
}