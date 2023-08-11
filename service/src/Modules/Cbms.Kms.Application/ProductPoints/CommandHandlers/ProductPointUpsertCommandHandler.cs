using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductPoints.Commands;
using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Kms.Application.ProductPoints.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Domain.ProductPoints.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductPoints.CommandHandlers
{
    public class ProductPointUpsertCommandHandler : UpsertEntityCommandHandler<ProductPointUpsertCommand, ProductPointGet, ProductPointDto>
    {
        private readonly IRepository<ProductPoint, int> _productPointRepository;

        public ProductPointUpsertCommandHandler(IRequestSupplement supplement, IRepository<ProductPoint, int> ProductPointRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _productPointRepository = ProductPointRepository;
        }

        protected override async Task<ProductPointDto> HandleCommand(ProductPointUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            ProductPoint entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _productPointRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = new ProductPoint();
                await _productPointRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new ProductPointUpsertAction(
                IocResolver,
                LocalizationSource,
                entityDto.ProductId,
                entityDto.Points,
                entityDto.FromDate,
                entityDto.ToDate,
                entityDto.IsActive,
                false
            ));

            await _productPointRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}