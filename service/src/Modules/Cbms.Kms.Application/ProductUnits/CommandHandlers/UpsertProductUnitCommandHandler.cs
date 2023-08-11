using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.ProductUnits.Commands;
using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Kms.Application.ProductUnits.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Kms.Domain.ProductUnits.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductUnits.CommandHandlers
{
    public class UpsertProductUnitCommandHandler : UpsertEntityCommandHandler<UpsertProductUnitCommand, GetProductUnit, ProductUnitDto>
    {
        private readonly IRepository<ProductUnit, int> _unitRepository;

        public UpsertProductUnitCommandHandler(IRequestSupplement supplement, IRepository<ProductUnit, int> unitRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _unitRepository = unitRepository;
        }

        protected override async Task<ProductUnitDto> HandleCommand(UpsertProductUnitCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            ProductUnit entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _unitRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = ProductUnit.Create();
                await _unitRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new UpsertProductUnitAction(entityDto.Code, entityDto.Name, entityDto.IsActive));
            await _unitRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}