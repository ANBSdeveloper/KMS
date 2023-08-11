using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Commands;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Budgets.Actions;
using Cbms.Kms.Domain.Cycles;
using Cbms.Mediator;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.CommandHandlers
{
    public class BudgetUpsertCommandHandler : UpsertEntityCommandHandler<BudgetUpsertCommand, BudgetGet, BudgetDto>
    {
        private readonly IRepository<Budget, int> _budgetRepository;
        private readonly IRepository<Cycle, int> _cycleRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public BudgetUpsertCommandHandler(
            IRequestSupplement supplement,
            IRepository<Budget, int> budgetRepository,
            IRepository<Cycle, int> cycleRepository,
            DistributedLockManager distributedLockManager) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;

            _budgetRepository = budgetRepository;
            _cycleRepository = cycleRepository;
            _distributedLockManager = distributedLockManager;
        }

        protected override async Task<BudgetDto> HandleCommand(BudgetUpsertCommand request, CancellationToken cancellationToken)
        {
            await using (await _distributedLockManager.AcquireAsync($"budget"))
            {
                var entityDto = request.Data;

                Budget entity = null;
                if (!request.Data.Id.IsNew())
                {
                    entity = await _budgetRepository
                        .GetAllIncluding(p => p.Zones, p => p.Areas, p => p.Branches)
                        .FirstOrDefaultAsync(p => p.Id == entityDto.Id);

                    if (entity == null)
                    {
                        throw new EntityNotFoundException(typeof(Budget), entityDto.Id);
                    }

                    await entity.ApplyActionAsync(new BudgetUpsertAction(
                        entityDto.CycleId,
                        entityDto.InvestmentType,
                        entityDto.ZonesChanges.UpsertedItems.Select(p => new BudgetZoneUpsertAction(p.Id, p.ZoneId, p.AllocateAmount)).ToList(),
                        entityDto.ZonesChanges.DeletedItems.Select(p => p.Id).ToList(),
                        entityDto.AreasChanges.UpsertedItems.Select(p => new BudgetAreaUpsertAction(p.Id, p.AreaId, p.AllocateAmount)).ToList(),
                        entityDto.AreasChanges.DeletedItems.Select(p => p.Id).ToList(),
                        entityDto.BranchesChanges.UpsertedItems.Select(p => new BudgetBranchUpsertAction(p.Id, p.BranchId, p.AllocateAmount)).ToList(),
                        entityDto.BranchesChanges.DeletedItems.Select(p => p.Id).ToList()
                    ));
                }
                else
                {
                    entity = await _budgetRepository
                       .FirstOrDefaultAsync(p => p.InvestmentType == entityDto.InvestmentType && p.CycleId == entityDto.CycleId);
                    if (entity != null)
                    {
                        var cycle = await _cycleRepository.GetAsync(entityDto.CycleId);
                        throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Budget.Exists", cycle.Number).Build();
                    }

                    entity = Budget.Create();

                    await entity.ApplyActionAsync(new BudgetUpsertAction(
                        entityDto.CycleId,
                        entityDto.InvestmentType,
                        entityDto.ZonesChanges.UpsertedItems.Select(p => new BudgetZoneUpsertAction(p.Id, p.ZoneId, p.AllocateAmount)).ToList(),
                        entityDto.ZonesChanges.DeletedItems.Select(p => p.Id).ToList(),
                        entityDto.AreasChanges.UpsertedItems.Select(p => new BudgetAreaUpsertAction(p.Id, p.AreaId, p.AllocateAmount)).ToList(),
                        entityDto.AreasChanges.DeletedItems.Select(p => p.Id).ToList(),
                        entityDto.BranchesChanges.UpsertedItems.Select(p => new BudgetBranchUpsertAction(p.Id, p.BranchId, p.AllocateAmount)).ToList(),
                        entityDto.BranchesChanges.DeletedItems.Select(p => p.Id).ToList()
                    ));

                    await _budgetRepository.InsertAsync(entity);
                }

                await _budgetRepository.UnitOfWork.CommitAsync(cancellationToken);

                return await GetEntityDtoAsync(entity.Id);
            }
        }
    }
}