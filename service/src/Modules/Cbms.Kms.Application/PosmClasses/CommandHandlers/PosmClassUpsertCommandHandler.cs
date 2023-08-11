using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmClasses.Commands;
using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Kms.Application.PosmClasses.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmClasses.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmClasses.CommandHandlers
{
    public class PosmClassUpsertCommandHandler : UpsertEntityCommandHandler<PosmClassUpsertCommand, PosmClassGet, PosmClassDto>
    {
        private readonly IRepository<PosmClass, int> _vendorRepository;

        public PosmClassUpsertCommandHandler(IRequestSupplement supplement, IRepository<PosmClass, int> vendorRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _vendorRepository = vendorRepository;
        }

        protected override async Task<PosmClassDto> HandleCommand(PosmClassUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            PosmClass entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _vendorRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = PosmClass.Create();
                await _vendorRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new PosmClassUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.IncludeInfo,
                entityDto.IsActive
            ));

            await _vendorRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}