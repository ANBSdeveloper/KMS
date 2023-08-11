using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Products.Commands;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.Products.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Products.CommandHandlers
{
    public class UpsertProductItemCommandHandler : UpsertEntityCommandHandler<UpsertProductItemCommand, GetProductItem, ProductItemDto>
    {
        private readonly IRepository<Product, int> _productRepository;

        public UpsertProductItemCommandHandler(IRequestSupplement supplement, IRepository<Product, int> ProductRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _productRepository = ProductRepository;
        }

        protected override async Task<ProductItemDto> HandleCommand(UpsertProductItemCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Product entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _productRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = Product.Create();
                await _productRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new UpsertProductAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.Description,
                entityDto.Unit,
                entityDto.CaseUnit,
                entityDto.PackSize,
                entityDto.ProductClassId,
                entityDto.SubProductClassId,
                entityDto.BrandId,
                entityDto.UpdateDate,
                entityDto.IsActive
            ));

            await _productRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}
