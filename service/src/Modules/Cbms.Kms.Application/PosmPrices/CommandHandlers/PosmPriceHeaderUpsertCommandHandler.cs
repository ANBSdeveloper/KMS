using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmPrices.Commands;
using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Kms.Application.PosmPrices.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Kms.Domain.PosmPrices.Actions;
using Cbms.Mediator;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmPrices.CommandHandlers
{
    public class PosmPriceUpsertCommandHandler : UpsertEntityCommandHandler<PosmPriceUpsertCommand, PosmPriceHeaderGet, PosmPriceHeaderDto>
    {
        private readonly IRepository<PosmPriceHeader, int> _posmPriceRepository;
        public PosmPriceUpsertCommandHandler(
            IRequestSupplement supplement,
            IRepository<PosmPriceHeader, int> posmPriceRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmPriceRepository = posmPriceRepository;
        }

        protected override async Task<PosmPriceHeaderDto> HandleCommand(PosmPriceUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            PosmPriceHeader entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _posmPriceRepository
                    .GetAllIncluding(p => p.PosmPriceDetails)
                    .FirstOrDefaultAsync(p => p.Id == entityDto.Id);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(PosmPriceHeader), entityDto.Id);
                }
            }
            else
            {
                entity = await _posmPriceRepository
                    .GetAllIncluding(p => p.PosmPriceDetails)
                    .FirstOrDefaultAsync(p => p.Id == entityDto.Id);
                if (entity != null)
                {
                    //var cycle = await _cycleRepository.GetAsync(entityDto.CycleId);
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("PosmPrice.Exists", entityDto.Code).Build();
                }

                entity = PosmPriceHeader.Create();
                await _posmPriceRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new PosmPriceUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.FromDate,
                entityDto.ToDate,
                entityDto.IsActive,
                entityDto.DetailChanges.UpsertedItems.Select(p => new PosmPriceDetailUpsertAction(p.Id, p.PosmItemId, p.Price)).ToList(),
                entityDto.DetailChanges.DeletedItems.Select(p => p.Id).ToList()
            ));

            await _posmPriceRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}