using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.QueryHandlers
{
    public class BudgetGetListHandler : QueryHandlerBase, IRequestHandler<BudgetGetList, PagingResult<BudgetListItemDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepository<Staff, int> _staffRepsitory;

        public BudgetGetListHandler(IRequestSupplement supplement, AppDbContext dbContext, IRepository<Staff, int> staffRepository) : base(supplement)
        {
            _dbContext = dbContext;
            _staffRepsitory = staffRepository;
        }

        public async Task<PagingResult<BudgetListItemDto>> Handle(BudgetGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            IQueryable<GroupBudgetDetail> groupQuery;

            var isManageBudget = Session.Permissions.Any(p => p.Equals("Budgets", System.StringComparison.CurrentCultureIgnoreCase));
            var isAllocateAreaBudget = Session.Permissions.Any(p => p.Equals("Budgets.AllocateArea", System.StringComparison.CurrentCultureIgnoreCase));
            var isAllocateBranchBudget = Session.Permissions.Any(p => p.Equals("Budgets.AllocateBranch", System.StringComparison.CurrentCultureIgnoreCase));

          
            if (isManageBudget)
            {
                groupQuery = from p in (
                                from budget in _dbContext.Budgets
                                join budgetZone in _dbContext.BudgetZones on budget.Id equals budgetZone.BudgetId
                                select new GroupBudgetDetail()
                                {
                                    Id = budget.Id,
                                    AllocatedAmount = budgetZone.AllocateAmount,
                                    UsedAmount = budgetZone.UsedAmount,
                                    RemainAmount = budgetZone.RemainAmount,
                                    TempRemainAmount = budgetZone.TempRemainAmount,
                                    TempUsedAmount = budgetZone.TempUsedAmount
                                }
                             )
                             group p by p.Id into g
                             select new GroupBudgetDetail
                             {
                                 Id = g.Key,
                                 AllocatedAmount = g.Sum(x => x.AllocatedAmount),
                                 UsedAmount = g.Sum(x => x.UsedAmount),
                                 RemainAmount = g.Sum(x => x.RemainAmount),
                                 TempUsedAmount = g.Sum(x => x.TempUsedAmount),
                                 TempRemainAmount = g.Sum(x => x.TempRemainAmount),
                             };
            }
            else if (isAllocateAreaBudget)
            {
                var staff = await _staffRepsitory.GetAll().FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                groupQuery = from p in (
                                 from budget in _dbContext.Budgets
                                 join budgetZone in _dbContext.BudgetZones on budget.Id equals budgetZone.BudgetId
                                 where budgetZone.ZoneId == staff.ZoneId
                                 select new GroupBudgetDetail()
                                 {
                                     Id = budget.Id,
                                     AllocatedAmount = budgetZone.AllocateAmount,
                                     UsedAmount = budgetZone.UsedAmount,
                                     RemainAmount = budgetZone.RemainAmount,
                                     TempRemainAmount = budgetZone.TempRemainAmount,
                                     TempUsedAmount = budgetZone.TempUsedAmount
                                 }
                             )
                             group p by p.Id into g
                             select new GroupBudgetDetail
                             {
                                 Id = g.Key,
                                 AllocatedAmount = g.Sum(x => x.AllocatedAmount),
                                 UsedAmount = g.Sum(x => x.UsedAmount),
                                 RemainAmount = g.Sum(x => x.RemainAmount),
                                 TempUsedAmount = g.Sum(x => x.TempUsedAmount),
                                 TempRemainAmount = g.Sum(x => x.TempRemainAmount),
                             };
            }
            else
            {
                var staff = await _staffRepsitory.GetAll().FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                groupQuery = from p in (
                                 from budget in _dbContext.Budgets
                                 join budgetArea in _dbContext.BudgetAreas on budget.Id equals budgetArea.BudgetId
                                 where budgetArea.AreaId == staff.AreaId
                                 select new GroupBudgetDetail()
                                 {
                                     Id = budget.Id,
                                     AllocatedAmount = budgetArea.AllocateAmount,
                                     UsedAmount = budgetArea.UsedAmount,
                                     RemainAmount = budgetArea.RemainAmount,
                                     TempRemainAmount = budgetArea.TempRemainAmount,
                                     TempUsedAmount = budgetArea.TempUsedAmount
                                 }
                             )
                             group p by p.Id into g
                             select new GroupBudgetDetail
                             {
                                 Id = g.Key,
                                 AllocatedAmount = g.Sum(x => x.AllocatedAmount),
                                 UsedAmount = g.Sum(x => x.UsedAmount),
                                 RemainAmount = g.Sum(x => x.RemainAmount),
                                 TempUsedAmount = g.Sum(x => x.TempUsedAmount),
                                 TempRemainAmount = g.Sum(x => x.TempRemainAmount),
                             };
            }

            var query = from budget in _dbContext.Budgets 
                        join cycle in _dbContext.Cycles on budget.CycleId equals cycle.Id
                        join g in groupQuery on budget.Id equals g.Id into groupD
                        from g in groupD.DefaultIfEmpty()
                        select new BudgetListItemDto()
                        {
                            Id = budget.Id,
                            AllocateAmount = g.AllocatedAmount,
                            CreationTime = budget.CreationTime,
                            CreatorUserId = budget.CreatorUserId,
                            CycleId = budget.CycleId,
                            CycleNumber = cycle.Number,
                            LastModificationTime = budget.LastModificationTime,
                            LastModifierUserId = budget.LastModifierUserId,
                            InvestmentType = (int)budget.InvestmentType,
                            RemainAmount = g.RemainAmount,
                            UsedAmount = g.UsedAmount,
                            TempRemainAmount = g.TempRemainAmount,
                            TempUsedAmount = g.TempUsedAmount
                        };

            query = query
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.CycleNumber.Contains(keyword))
                .WhereIf(request.InvestmentType.HasValue, x => x.InvestmentType == (int)request.InvestmentType);

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<BudgetListItemDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }

        public class GroupBudgetDetail
        {
            public decimal AllocatedAmount { get; set; }
            public int Id { get; set; }
            public decimal RemainAmount { get; set; }
            public decimal UsedAmount { get; set; }
            public decimal TempUsedAmount { get; set; }
            public decimal TempRemainAmount { get; set; }
        }
    }
}