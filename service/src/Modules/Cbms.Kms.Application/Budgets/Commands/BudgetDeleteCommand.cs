using Cbms.Mediator;

namespace Cbms.Kms.Application.Budgets.Commands
{
    public class BudgetDeleteCommand : DeleteEntityCommand
    {
        public BudgetDeleteCommand(int id) : base(id)
        {
        }
    }
}