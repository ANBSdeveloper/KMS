using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductClasses.Commands;
using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Kms.Application.ProductClasses.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Kms.Domain.ProductClasses.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductClasses.CommandHandlers
{
    public class UpsertProductClassCommandHandler : UpsertEntityCommandHandler<UpsertProductClassCommand, GetProductClass, ProductClassDto>
    {
        private readonly IRepository<ProductClass, int> _productClassRepository;

        public UpsertProductClassCommandHandler(IRequestSupplement supplement, IRepository<ProductClass, int> productClassRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _productClassRepository = productClassRepository;
        }

        protected override async Task<ProductClassDto> HandleCommand(UpsertProductClassCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            ProductClass entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _productClassRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = ProductClass.Create();
                await _productClassRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(
                new ProductClassUpsertAction(
                    entityDto.Code,
                    entityDto.Name,
                    entityDto.RewardCode,
                    entityDto.IsActive)
            );
            await _productClassRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}