using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Domain.Budgets;
using Cbms.Mediator;
using MediatR;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Budgets.Query
{
    public class GetBudgetHistoryByUser : QueryBase, IRequest<List<BudgetHistoryByUserDto>>
    {

        public GetBudgetHistoryByUser() : base()
        {
        }
        public BudgetInvestmentType Type { get; set; }
        public int CycleId { get; set; }
    }
}