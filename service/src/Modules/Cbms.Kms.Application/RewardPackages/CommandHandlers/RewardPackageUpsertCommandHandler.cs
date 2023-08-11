using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.RewardPackages.Commands;
using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Kms.Application.RewardPackages.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.RewardPackages.Actions;
using Cbms.Mediator;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.RewardPackages.CommandHandlers
{
    public class RewardPackageUpsertCommandHandler : UpsertEntityCommandHandler<RewardPackageUpsertCommand, GetRewardPackage, RewardPackageDto>
    {
        private readonly IRepository<RewardPackage, int> _rewardPackageRepository;

        //private readonly IRepository<Cycle, int> _cycleRepository;
        public RewardPackageUpsertCommandHandler(
            IRequestSupplement supplement,
            IRepository<RewardPackage, int> rewardPackageRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;

            _rewardPackageRepository = rewardPackageRepository;
        }

        protected override async Task<RewardPackageDto> HandleCommand(RewardPackageUpsertCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            RewardPackage entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _rewardPackageRepository
                    .GetAllIncluding(p => p.RewardItems, prop=>prop.RewardBranches)
                    .FirstOrDefaultAsync(p => p.Id == entityDto.Id);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(RewardPackage), entityDto.Id);
                }
            }
            else
            {
                entity = await _rewardPackageRepository
                   .FirstOrDefaultAsync(p => p.Code == entityDto.Code);
                if (entity != null)
                {
                    //var cycle = await _cycleRepository.GetAsync(entityDto.CycleId);
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("RewardPackage.Exists", entityDto.Code).Build();
                }

                entity = RewardPackage.Create();
                await _rewardPackageRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new RewardPackageUpsertAction(
                entityDto.Code,
                entityDto.Name,
                entityDto.IsActive,
                (RewardPackageType)entityDto.Type,
                entityDto.FromDate,
                entityDto.ToDate,
                entityDto.RewardItemChanges.UpsertedItems.Select(p => new RewardItemUpsertAction(p.Id, p.Code, p.Name, p.DocumentLink, p.ProductUnitId, p.Price, p.Quantity, p.ProductId)).ToList(),
                entityDto.RewardItemChanges.DeletedItems.Select(p => p.Id).ToList(),
                entityDto.RewardBranchChanges.UpsertedItems.Select(p => new RewardBranchUpsertAction(p.Id, p.BranchId)).ToList(),
                entityDto.RewardBranchChanges.DeletedItems.Select(p => p.Id).ToList()
            ));

            await _rewardPackageRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}