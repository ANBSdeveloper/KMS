using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmTypes.Commands;
using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Kms.Application.PosmTypes.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmTypes;
using Cbms.Kms.Domain.PosmTypes.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmTypes.CommandHandlers
{
    public class PosmTypeUpsertCommandHandler : UpsertEntityCommandHandler<PosmTypeUpsertCommand, PosmTypeGet, PosmTypeDto>
    {
        private readonly IRepository<PosmType, int> _vendorRepository;

        public PosmTypeUpsertCommandHandler(IRequestSupplement supplement, IRepository<PosmType, int> vendorRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _vendorRepository = vendorRepository;
        }

        protected override async Task<PosmTypeDto> HandleCommand(PosmTypeUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            PosmType entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _vendorRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = PosmType.Create();
                await _vendorRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new PosmTypeUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.IsActive
            ));

            await _vendorRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}