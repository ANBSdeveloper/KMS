using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Application.Budgets.Query;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.QueryHandler
{
    public class BudgetDetailGetByIdHandler : QueryHandlerBase, IRequestHandler<BudgetDetailGetById, BudgetDetailGetByIdDto>
    {
        private readonly AppDbContext _dbContext;
    

        public BudgetDetailGetByIdHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
     
            _dbContext = dbContext;
        }

        public async Task<BudgetDetailGetByIdDto> Handle(BudgetDetailGetById request, CancellationToken cancellationToken)
        {
            var staffEntity = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
            if (staffEntity == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Budget.UserInvalid").Build();
            }

            var entity = await (from budgetDetail in _dbContext.BudgetAreas
                                  join budget in _dbContext.Budgets on budgetDetail.BudgetId equals budget.Id
                                  join cycle in _dbContext.Cycles on budget.CycleId equals cycle.Id
                             
                                  where budgetDetail.AreaId == staffEntity.AreaId && budget.CycleId == request.CycleId      && (int)budget.InvestmentType == request.InvestmentType
                                select new BudgetDetailBase()
                                  {
                                      BudgetId = budgetDetail.BudgetId,
                                      CycleId = cycle.Id,
                                      FromDate = cycle.FromDate,
                                      ToDate = cycle.ToDate,
                                      InvestmentType = (int)budget.InvestmentType,
                                      StaffId = budgetDetail.StaffId,
                                      StaffCode = staff.Code,
                                      StaffName = staff.Name,
                                      AllocateAmount = budgetDetail.AllocateAmount,
                                      RemainAmount = budgetDetail.RemainAmount,
                                      UsedAmount = budgetDetail.UsedAmount,
                                      TempRemainAmount = budgetDetail.TempRemainAmount,
                                      TempUsedAmount = budgetDetail.TempUsedAmount,
                                      Point = staff.CreditPoint
                                  }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(BudgetDetailGetByIdDto), request.CycleId);
            }

            var ticketInvestmentHistoryByBudgetIdDtos = await (
                                        from ticketInvestment in _dbContext.TicketInvestments
                                            .Include(p=>p.ConsumerRewards)
                                            .Include(p=>p.RewardItems)
                                        join customer in _dbContext.Customers on ticketInvestment.CustomerId equals customer.Id
                                        where ticketInvestment.BudgetId == entity.BudgetId && ticketInvestment.RegisterStaffId == entity.StaffId
                                        select new
                                        {
                                            ActualInvestmentAmount = 0,
                                            ConsumerRewards = ticketInvestment.ConsumerRewards.ToList(),
                                            RewardItems =ticketInvestment.RewardItems.ToList(),
                                            Id = ticketInvestment.Id,
                                            Code = ticketInvestment.Code,
                                            CustomerId = ticketInvestment.CustomerId,
                                            CustomerCode = customer.Code,
                                            CustomerName = customer.Name,
                                            Address = customer.Address,
                                            CreationTime = ticketInvestment.CreationTime,
                                            ApprovalDate = ticketInvestment.CreationTime,
                                            AcceptanceDate = ticketInvestment.CreationTime,
                                            InvestmentAmount = ticketInvestment.InvestmentAmount,
                                            InvestmentType = entity.InvestmentType,
                                            Status = (int)ticketInvestment.Status
                                        }).ToListAsync();

      
            var entityDto = new BudgetDetailGetByIdDto();
            entityDto.BudgetId = entity.BudgetId;
            entityDto.CycleId = entity.CycleId;
            entityDto.FromDate = entity.FromDate;
            entityDto.ToDate = entity.ToDate;
            entityDto.InvestmentType = entity.InvestmentType;
            entityDto.StaffId = entity.StaffId;
            entityDto.StaffCode = entity.StaffCode;
            entityDto.StaffName = entity.StaffName;
            entityDto.AllocateAmount = entity.AllocateAmount;
            entityDto.RemainAmount = entity.RemainAmount;
            entityDto.UsedAmount = entity.UsedAmount;
            entityDto.TempRemainAmount = entity.TempRemainAmount;
            entityDto.TempUsedAmount = entity.TempUsedAmount;
            entityDto.Point = entity.Point;
            entityDto.TicketInvestmentHistory = ticketInvestmentHistoryByBudgetIdDtos
                .Select(item => new TicketInvestmentHistoryByBudgetIdDto() {
                    ActualInvestmentAmount = item.ConsumerRewards
                                                    .Sum(consumerReward => consumerReward.RewardQuantity * 
                                                    item.RewardItems.FirstOrDefault(rewardItem => consumerReward.RewardItemId == rewardItem.RewardItemId).Price),
                    Id = item.Id,
                    Code = item.Code,
                    CustomerId = item.CustomerId,
                    CustomerCode = item.CustomerCode,
                    CustomerName = item.CustomerName,
                    Address = item.Address,
                    CreationTime = item.CreationTime,
                    ApprovalDate = item.CreationTime,
                    AcceptanceDate = item.CreationTime,
                    InvestmentAmount = item.InvestmentAmount,
                    InvestmentType = entity.InvestmentType,
                    Status = (int)item.Status
                }).ToList();
            
            return entityDto;
        }
    }
}