using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.QueryHandlers
{
    public class BudgetGetInitDetailHandler : QueryHandlerBase, IRequestHandler<BudgetGetInitDetail, BudgetInitDetailDto>
    {
        private readonly AppDbContext _dbContext;

        public BudgetGetInitDetailHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<BudgetInitDetailDto> Handle(BudgetGetInitDetail request, CancellationToken cancellationToken)
        {
            var zones = from zone in _dbContext.Zones
                        join org in _dbContext.SalesOrgs on zone.SalesOrgId equals org.Id
                        select new BudgetZoneDto()
                        {
                            ZoneId = zone.Id,
                            ZoneCode = zone.Code,
                            ZoneName = zone.Name,
                            SalesOrgId = org.Id,
                            ParentSalesOrgId = org.ParentId,
                        };

            var areas = 
                        from area in _dbContext.Areas
                        join zone in _dbContext.Zones on area.ZoneId equals zone.Id
                        join org in _dbContext.SalesOrgs on area.SalesOrgId equals org.Id
                        select new BudgetAreaDto()
                        {
                            ZoneId = area.ZoneId,
                            ZoneName = zone.Name,
                            AreaId = area.Id,
                            AreaCode = area.Code,
                            AreaName = area.Name,
                            SalesOrgId = org.Id,
                            ParentSalesOrgId = org.ParentId,
                        };

            var branches =
                        from branch in _dbContext.Branches
                        join area in _dbContext.Areas on branch.AreaId equals area.Id
                        join zone in _dbContext.Zones on branch.ZoneId equals zone.Id
                        join org in _dbContext.SalesOrgs on branch.SalesOrgId equals org.Id
                        where branch.AreaId.HasValue && branch.ZoneId.HasValue
                        select new BudgetBranchDto()
                        {
                            ZoneId = branch.ZoneId.Value,
                            ZoneName = zone.Name,
                            AreaId = branch.AreaId.Value,
                            AreaName = area.Name,
                            BranchId = branch.Id,
                            BranchCode = branch.Code,
                            BranchName = branch.Name,
                            SalesOrgId = org.Id,
                            ParentSalesOrgId = org.ParentId,
                        };


            return new BudgetInitDetailDto()
            {
                Zones = await zones.ToListAsync(),
                Areas = await areas.ToListAsync(),
                Branches = await branches.ToListAsync()
            };
        }
    }
}