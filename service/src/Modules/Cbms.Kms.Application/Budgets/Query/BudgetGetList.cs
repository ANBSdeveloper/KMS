using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Domain.Budgets;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Budgets.Query
{
    public class BudgetGetList : EntityPagingResultQuery<BudgetListItemDto>
    {
        public BudgetInvestmentType? InvestmentType { get; set; }
    }
}