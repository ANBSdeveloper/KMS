using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.QueryHandler
{
    public class GetBudgetHistoryByUserHandler : QueryHandlerBase, IRequestHandler<GetBudgetHistoryByUser, List<BudgetHistoryByUserDto>>
    {
        private readonly AppDbContext _dbContext;
        public GetBudgetHistoryByUserHandler(
            IRequestSupplement supplement, 
            AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<List<BudgetHistoryByUserDto>> Handle(GetBudgetHistoryByUser request, CancellationToken cancellationToken)
        {
            var staffEntity = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
            if (staffEntity == null)
            {
                throw  BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Budget.UserInvalid").Build();
            }

            var details = new List<BudgetHistoryByUserDto>();
            var ticketBudget = await GetBudget(request.CycleId, staffEntity, BudgetInvestmentType.BTTT);
            if (ticketBudget != null) details.Add(ticketBudget);

            var posmBudget = await GetBudget(request.CycleId, staffEntity, BudgetInvestmentType.POSM);
            if (posmBudget != null) details.Add(posmBudget);

            return details;
        }

        private async Task<BudgetHistoryByUserDto> GetBudget(int cycleId, Staff staffEntity, BudgetInvestmentType type)
        {
            BudgetHistoryByUserDto summaryBudget = null;
            if (staffEntity.StaffTypeCode == KmsConsts.RsmRole)
            {
                summaryBudget = await(from budgetDetail in _dbContext.BudgetZones
                                      join budget in _dbContext.Budgets on budgetDetail.BudgetId equals budget.Id
                                      join cycle in _dbContext.Cycles on budget.CycleId equals cycle.Id
                                      where budgetDetail.ZoneId == staffEntity.ZoneId && budget.CycleId == cycleId
                                      && budget.InvestmentType == type
                                      select new BudgetHistoryByUserDto()
                                      {
                                          InvestmentType = (int)budget.InvestmentType,
                                          AllocateAmount = budgetDetail.AllocateAmount,
                                          RemainAmount = budgetDetail.RemainAmount,
                                          UsedAmount = budgetDetail.UsedAmount,
                                          TempRemainAmount = budgetDetail.TempRemainAmount,
                                          TempUsedAmount = budgetDetail.TempUsedAmount,
                                      }).FirstOrDefaultAsync();
            }
            else if (staffEntity.StaffTypeCode == KmsConsts.AsmRole)
            {
                summaryBudget = await(from budgetDetail in _dbContext.BudgetAreas
                                      join budget in _dbContext.Budgets on budgetDetail.BudgetId equals budget.Id
                                      join cycle in _dbContext.Cycles on budget.CycleId equals cycle.Id
                                      where budgetDetail.AreaId == staffEntity.AreaId && budget.CycleId == cycleId
                                      && budget.InvestmentType == type
                                      select new BudgetHistoryByUserDto()
                                      {
                                          InvestmentType = (int)budget.InvestmentType,
                                          AllocateAmount = budgetDetail.AllocateAmount,
                                          RemainAmount = budgetDetail.RemainAmount,
                                          UsedAmount = budgetDetail.UsedAmount,
                                          TempRemainAmount = budgetDetail.TempRemainAmount,
                                          TempUsedAmount = budgetDetail.TempUsedAmount,
                                      }).FirstOrDefaultAsync();
            }
            else
            {
                summaryBudget = await(from budgetDetail in _dbContext.BudgetBranches
                                      join branch in _dbContext.Branches on budgetDetail.BranchId equals branch.Id
                                      join budget in _dbContext.Budgets on budgetDetail.BudgetId equals budget.Id
                                      join cycle in _dbContext.Cycles on budget.CycleId equals cycle.Id
                                      where branch.SalesOrgId == staffEntity.SalesOrgId && budget.CycleId == cycleId
                                      && budget.InvestmentType == type
                                      select new BudgetHistoryByUserDto()
                                      {
                                          InvestmentType = (int)budget.InvestmentType,
                                          AllocateAmount = budgetDetail.AllocateAmount,
                                          RemainAmount = budgetDetail.RemainAmount,
                                          UsedAmount = budgetDetail.UsedAmount,
                                          TempRemainAmount = budgetDetail.TempRemainAmount,
                                          TempUsedAmount = budgetDetail.TempUsedAmount,
                                      }).FirstOrDefaultAsync();
            }



            if (summaryBudget != null)
            {
                if (type == BudgetInvestmentType.BTTT)
                {
                    var tickets = await Mediator.Send(new TicketInvestmnetGetListByUser()
                    {
                        CycleId = cycleId,
                        StaffId = staffEntity.Id,
                        Status = new List<int>() {
                        (int)TicketInvestmentStatus.Approved,
                        (int)TicketInvestmentStatus.Doing,
                        (int)TicketInvestmentStatus.Accepted,
                        (int)TicketInvestmentStatus.Operated,
                        (int)TicketInvestmentStatus.FinalSettlement,
                    }
                    });
                    summaryBudget.Calls = tickets.Items.GroupBy(p => p.CustomerId).Count();
                }

            }

            return summaryBudget;
        }
    }
}