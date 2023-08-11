using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Budgets.Actions;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Zones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Budgets
{
    public class Budget : AuditedAggregateRoot
    {
        public List<BudgetArea> _areas = new List<BudgetArea>();
        public List<BudgetBranch> _branches = new List<BudgetBranch>();
        public List<BudgetZone> _zones = new List<BudgetZone>();
        private Budget()
        {
        }
        public IReadOnlyCollection<BudgetZone> Zones => _zones;
        public IReadOnlyCollection<BudgetArea> Areas => _areas;
        public IReadOnlyCollection<BudgetBranch> Branches => _branches;
        public int CycleId { get; private set; }
        public BudgetInvestmentType InvestmentType { get; private set; }
       
        public static Budget Create()
        {
            return new Budget();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case BudgetUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;

                case BudgetTemporaryUseAction temporaryUseAction:
                    await TemporaryUseAsync(temporaryUseAction);
                    break;

                case BudgetUseAction useAction:
                    await UseAsync(useAction);
                    break;
            }
        }

        public async Task TemporaryUseAsync(BudgetTemporaryUseAction action)
        {
            async Task UseAreaAsync(int areaId)
            {
                var budgetArea = _areas.FirstOrDefault(p => p.AreaId == areaId);

                if (budgetArea == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    var area = await action.IocResolver.Resolve<IRepository<Area, int>>().GetAsync(areaId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForArea", cycle.Number, area.Name)
                    .Build();
                }

                await budgetArea.ApplyActionAsync(
                   new BudgetAreaTemporaryUseAction(
                       action.IocResolver,
                   action.LocalizationSource,
                   action.UseAmount)
               );
            };

            async Task UseZoneAsync(int zoneId)
            {
                var budgetZone = _zones.FirstOrDefault(p => p.ZoneId == zoneId);
                if (budgetZone == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    var zone = await action.IocResolver.Resolve<IRepository<Zone, int>>().GetAsync(zoneId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForZone", cycle.Number, zone.Name)
                    .Build();
                }
                await budgetZone.ApplyActionAsync(
                   new BudgetZoneTemporaryUseAction(
                       action.IocResolver,
                   action.LocalizationSource,
                   action.UseAmount)
               );
            };

            if (action.LevelType == BudgetLevelType.Branch)
            { 
                var branchRepository = action.IocResolver.Resolve<IRepository<Branch, int>>();
                var branch = await branchRepository.GetAsync(action.ObjectId);

                var budgetBranch = _branches.FirstOrDefault(p => p.BranchId == action.ObjectId);

                if (budgetBranch == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForBranch", cycle.Number, branch.Name)
                    .Build();
                }

                await budgetBranch.ApplyActionAsync(
                    new BudgetBranchTemporaryUseAction(
                        action.IocResolver,
                    action.LocalizationSource,
                    action.UseAmount)
                );

            
                if (branch.AreaId.HasValue)
                {
                    await UseAreaAsync(branch.AreaId.Value);
                }

                if (branch.ZoneId.HasValue)
                {
                    await UseZoneAsync(branch.ZoneId.Value);
                }
            }
            else if (action.LevelType == BudgetLevelType.Area)
            {
                await UseAreaAsync(action.ObjectId);
            }
            else if (action.LevelType == BudgetLevelType.Zone)
            {
                await UseZoneAsync(action.ObjectId);
            }
        }

        public async Task UpsertAsync(BudgetUpsertAction action)
        {
            if (Id.IsNew())
            {
                CycleId = action.CycleId;
                InvestmentType = action.PortfolioType;
            }

            foreach (var id in action.DeleteZones)
            {
                var zone = _zones.FirstOrDefault(p => p.Id == id);
                if (zone != null)
                {
                    _zones.Remove(zone);
                }
            }

            foreach (var item in action.UpsertZones)
            {
                BudgetZone zone = null;

                if (!item.Id.IsNew())
                {
                    zone = _zones.FirstOrDefault(p => p.Id == item.Id);
                    if (zone == null)
                    {
                        throw new EntityNotFoundException(typeof(BudgetZone), item.Id);
                    }
                }
                else
                {
                    zone = _zones.FirstOrDefault(p => p.ZoneId == item.ZoneId);
                    if (zone == null)
                    {
                        zone = BudgetZone.Create();
                        _zones.Add(zone);
                    }
                }

                await zone.ApplyActionAsync(item);
            }


            foreach (var id in action.DeleteAreas)
            {
                var area = _areas.FirstOrDefault(p => p.Id == id);
                if (area != null)
                {
                    _areas.Remove(area);
                }
            }

            foreach (var item in action.UpsertAreas)
            {
                BudgetArea area = null;

                if (!item.Id.IsNew())
                {
                    area = _areas.FirstOrDefault(p => p.Id == item.Id);
                    if (area == null)
                    {
                        throw new EntityNotFoundException(typeof(BudgetArea), item.Id);
                    }
                }
                else
                {
                    area = _areas.FirstOrDefault(p => p.AreaId == item.AreaId);
                    if (area == null)
                    {
                        area = BudgetArea.Create();
                        _areas.Add(area);
                    }
                }

                await area.ApplyActionAsync(item);
            }

            foreach (var id in action.DeleteBranches)
            {
                var branch = _branches.FirstOrDefault(p => p.Id == id);
                if (branch != null)
                {
                    _branches.Remove(branch);
                }
            }

            foreach (var item in action.UpsertBranches)
            {
                BudgetBranch branch = null;

                if (!item.Id.IsNew())
                {
                    branch = _branches.FirstOrDefault(p => p.Id == item.Id);
                    if (branch == null)
                    {
                        throw new EntityNotFoundException(typeof(BudgetBranch), item.Id);
                    }
                }
                else
                {
                    branch = _branches.FirstOrDefault(p => p.BranchId == item.BranchId);
                    if (branch == null)
                    {
                        branch = BudgetBranch.Create();
                        _branches.Add(branch);
                    }
                }

                await branch.ApplyActionAsync(item);
            }
        }
        public async Task UseAsync(BudgetUseAction action)
        {
            async Task UseAreaAsync(int areaId)
            {
                var budgetArea = _areas.FirstOrDefault(p => p.AreaId == areaId);

                if (budgetArea == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    var area = await action.IocResolver.Resolve<IRepository<Area, int>>().GetAsync(areaId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForArea", cycle.Number, area.Name)
                    .Build();
                }

                await budgetArea.ApplyActionAsync(
                   new BudgetAreaUseAction(
                       action.IocResolver,
                   action.LocalizationSource,
                   action.TemporaryAmount,
                   action.UseAmount)
               );
            };

            async Task UseZoneAsync(int zoneId)
            {
                var budgetZone = _zones.FirstOrDefault(p => p.ZoneId == zoneId);

                if (budgetZone == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    var zone = await action.IocResolver.Resolve<IRepository<Zone, int>>().GetAsync(zoneId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForZone", cycle.Number, zone.Name)
                    .Build();
                }

                await budgetZone.ApplyActionAsync(
                   new BudgetZoneUseAction(
                       action.IocResolver,
                   action.LocalizationSource,
                   action.TemporaryAmount,
                   action.UseAmount)
               );
            };

            if (action.LevelType == BudgetLevelType.Branch)
            {
                var branchRepository = action.IocResolver.Resolve<IRepository<Branch, int>>();
                var branch = await branchRepository.GetAsync(action.ObjectId);

                var budgetBranch = _branches.FirstOrDefault(p => p.BranchId == action.ObjectId);

                if (budgetBranch == null)
                {
                    var cycle = await action.IocResolver.Resolve<IRepository<Cycle, int>>().GetAsync(CycleId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode("Budget.NotValidForBranch", cycle.Number, branch.Name)
                    .Build();
                }

                await budgetBranch.ApplyActionAsync(
                    new BudgetBranchUseAction(
                        action.IocResolver,
                    action.LocalizationSource,
                    action.TemporaryAmount,
                    action.UseAmount)
                );

                if (branch.AreaId.HasValue)
                {
                    await UseAreaAsync(branch.AreaId.Value);
                }

                if (branch.ZoneId.HasValue)
                {
                    await UseZoneAsync(branch.ZoneId.Value);
                }
            }
            else if (action.LevelType == BudgetLevelType.Area)
            {
                await UseAreaAsync(action.ObjectId);
            }
            else if (action.LevelType == BudgetLevelType.Zone)
            {
                await UseZoneAsync(action.ObjectId);
            }
        }
    }
}