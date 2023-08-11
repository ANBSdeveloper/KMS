using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Budgets.Commands
{
    public class BudgetUpsertCommand : UpsertEntityCommand<BudgetUpsertDto, BudgetDto>
    {
        public BudgetUpsertCommand(BudgetUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}