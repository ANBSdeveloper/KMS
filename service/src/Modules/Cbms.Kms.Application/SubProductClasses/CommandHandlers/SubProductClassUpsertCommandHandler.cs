using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.SubProductClasses.Commands;
using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Kms.Application.SubProductClasses.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Kms.Domain.SubProductClasses.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.ProductClasses.CommandHandlers
{
    public class SubProductClassUpsertCommandHandler : UpsertEntityCommandHandler<SubProductClassUpsertCommand, SubProductClassGet, SubProductClassDto>
    {
        private readonly IRepository<SubProductClass, int> _subProductClassRepository;

        public SubProductClassUpsertCommandHandler(IRequestSupplement supplement, IRepository<SubProductClass, int> subProductClassRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _subProductClassRepository = subProductClassRepository;
        }

        protected override async Task<SubProductClassDto> HandleCommand(SubProductClassUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            SubProductClass entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _subProductClassRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = new SubProductClass();
                await _subProductClassRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(
                new SubProductClassUpsertAction(
                    entityDto.Code,
                    entityDto.Name,
                    entityDto.IsActive)
            );
            await _subProductClassRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}