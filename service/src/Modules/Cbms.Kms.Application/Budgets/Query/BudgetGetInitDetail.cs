using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Kms.Domain.Budgets;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Budgets.Query
{
    public class BudgetGetInitDetail : QueryBase, IRequest<BudgetInitDetailDto>
    {
        public BudgetInvestmentType? InvestmentType { get; set; }
    }
}