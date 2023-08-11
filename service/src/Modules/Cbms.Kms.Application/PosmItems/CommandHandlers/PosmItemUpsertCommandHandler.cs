using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmItems.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.PosmItems.Actions;
using Cbms.Mediator;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.CommandHandlers
{
    public class PosmItemUpsertCommandHandler : UpsertEntityCommandHandler<PosmItemUpsertCommand, PosmItemGet, PosmItemDto>
    {
        private readonly IRepository<PosmItem, int> _posmItemRepository;

        public PosmItemUpsertCommandHandler(IRequestSupplement supplement, IRepository<PosmItem, int> posmItemRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmItemRepository = posmItemRepository;
        }

        protected override async Task<PosmItemDto> HandleCommand(PosmItemUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            PosmItem entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _posmItemRepository
                     .GetAllIncluding(p => p.PosmCatalogs)
                     .FirstOrDefaultAsync(p => p.Id == entityDto.Id);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(PosmItem), entityDto.Id);
                }
            }

            if (entity == null)
            {
                entity = PosmItem.Create();
                await _posmItemRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new PosmItemUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.PosmClassId,
                entityDto.PosmTypeId,
                entityDto.IsActive,
                entityDto.Link,
                entityDto.UnitType,
                entityDto.CalcType,
                entityDto.CatalogChanges.UpsertedItems.Select(p => new PosmCatalogUpsertAction(p.Id, p.Code, p.Name, p.Link)).ToList(),
                entityDto.CatalogChanges.DeletedItems.Select(p => p.Id).ToList()
            ));

            await _posmItemRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}