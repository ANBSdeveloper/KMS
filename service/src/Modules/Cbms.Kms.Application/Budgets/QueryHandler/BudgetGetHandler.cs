using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.QueryHandlers
{
    public class BudgetGetHandler : QueryHandlerBase, IRequestHandler<BudgetGet, BudgetDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepository<Budget, int> _repository;

        public BudgetGetHandler(IRequestSupplement supplement, IRepository<Budget, int> repository, AppDbContext dbContext) : base(supplement)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<BudgetDto> Handle(BudgetGet request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAsync(request.Id);
            var zoneDtos = await (
                                    from zone in _dbContext.Zones 
                                    join org in _dbContext.SalesOrgs on zone.SalesOrgId equals org.Id
                                    join budget in _dbContext.BudgetZones on new { zone.Id, BudgetId = entity.Id } equals new { Id = budget.ZoneId, BudgetId = budget.BudgetId } into budgetDf
                                    from budget in budgetDf.DefaultIfEmpty()
                                    select new BudgetZoneDto()
                                    {
                                        AllocateAmount = budget.AllocateAmount,
                                        CreationTime = budget.CreationTime,
                                        CreatorUserId = budget.CreatorUserId,
                                        Id = budget.Id,
                                        LastModificationTime = budget.LastModificationTime,
                                        LastModifierUserId = budget.LastModifierUserId,
                                        RemainAmount = budget.RemainAmount,
                                        TempUsedAmount = budget.TempUsedAmount,
                                        TempRemainAmount = budget.TempRemainAmount,
                                        UsedAmount = budget.UsedAmount,
                                        ZoneId = zone.Id,
                                        ZoneCode = zone.Code,
                                        ZoneName = zone.Name,
                                        SalesOrgId = org.Id,
                                        ParentSalesOrgId = org.ParentId,
                                    }).ToListAsync();

            var areaDtos = await (
                                   from area in _dbContext.Areas
                                   join zone in _dbContext.Zones on area.ZoneId equals zone.Id
                                   join org in _dbContext.SalesOrgs on area.SalesOrgId equals org.Id
                                   join budget in _dbContext.BudgetAreas on new { area.Id, BudgetId = entity.Id } equals new { Id = budget.AreaId, BudgetId = budget.BudgetId } into budgetDf
                                   from budget in budgetDf.DefaultIfEmpty()
                                   select new BudgetAreaDto()
                                   {
                                       AllocateAmount = budget.AllocateAmount,
                                       CreationTime = budget.CreationTime,
                                       CreatorUserId = budget.CreatorUserId,
                                       Id = budget.Id,
                                       LastModificationTime = budget.LastModificationTime,
                                       LastModifierUserId = budget.LastModifierUserId,
                                       RemainAmount = budget.RemainAmount,
                                       TempUsedAmount = budget.TempUsedAmount,
                                       TempRemainAmount = budget.TempRemainAmount,
                                       UsedAmount = budget.UsedAmount,
                                       AreaId = area.Id,
                                       AreaCode = area.Code,
                                       AreaName = area.Name,
                                       ZoneId = zone.Id,
                                       ZoneName = zone.Name,
                                       SalesOrgId = org.Id,
                                       ParentSalesOrgId = org.ParentId,
                                   }).ToListAsync();

            var branchDtos = await (
                                   from branch in _dbContext.Branches 
                                   join area in _dbContext.Areas on branch.AreaId equals area.Id
                                   join zone in _dbContext.Zones on area.ZoneId equals zone.Id
                                   join org in _dbContext.SalesOrgs on area.SalesOrgId equals org.Id
                                   join budget in _dbContext.BudgetBranches on new { branch.Id, BudgetId = entity.Id } equals new { Id = budget.BranchId, BudgetId = budget.BudgetId } into budgetDf
                                   from budget in budgetDf.DefaultIfEmpty()
                                   select new BudgetBranchDto()
                                   {
                                       AllocateAmount = budget.AllocateAmount,
                                       CreationTime = budget.CreationTime,
                                       CreatorUserId = budget.CreatorUserId,
                                       Id = budget.Id,
                                       LastModificationTime = budget.LastModificationTime,
                                       LastModifierUserId = budget.LastModifierUserId,
                                       RemainAmount = budget.RemainAmount,
                                       TempUsedAmount = budget.TempUsedAmount,
                                       TempRemainAmount = budget.TempRemainAmount,
                                       UsedAmount = budget.UsedAmount,
                                       BranchId = branch.Id,
                                       BranchCode = branch.Code,
                                       BranchName = branch.Name,
                                       ZoneId = zone.Id,
                                       ZoneName = zone.Name,
                                       AreaId = area.Id,
                                       AreaName = area.Name,
                                       SalesOrgId = org.Id,
                                       ParentSalesOrgId = org.ParentId,
                                   }).ToListAsync();

            var entityDto = Mapper.Map<BudgetDto>(entity);

            var cycle = await (from p in _dbContext.Cycles where p.Id == entity.CycleId select p).FirstOrDefaultAsync();
            entityDto.FromDate = cycle.FromDate;
            entityDto.ToDate = cycle.ToDate;
            entityDto.Zones = zoneDtos;
            entityDto.Areas = areaDtos;
            entityDto.Branches = branchDtos;
            return entityDto;
        }
    }
}