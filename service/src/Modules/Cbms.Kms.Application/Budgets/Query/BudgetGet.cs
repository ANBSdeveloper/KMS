using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Budgets.Query
{
    public class BudgetGet : EntityQuery<BudgetDto>
    {
        public BudgetGet(int id) : base(id)
        {
        }
    }
}