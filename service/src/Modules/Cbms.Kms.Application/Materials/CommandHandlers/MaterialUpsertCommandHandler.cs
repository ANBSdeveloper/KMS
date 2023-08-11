using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Application.Materials.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.Materials.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class MaterialUpsertCommandHandler : UpsertEntityCommandHandler<MaterialUpsertCommand, MaterialGet, MaterialDto>
    {
        private readonly IRepository<Material, int> _materialRepository;

        public MaterialUpsertCommandHandler(IRequestSupplement supplement, IRepository<Material, int> MaterialRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _materialRepository = MaterialRepository;
        }

        protected override async Task<MaterialDto> HandleCommand(MaterialUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Material entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _materialRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = Material.Create();
                await _materialRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new UpsertMaterialAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.MaterialTypeId,
                entityDto.Description,
                entityDto.Value,
                entityDto.IsActive,
                entityDto.IsDesign
            ));

            await _materialRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}
