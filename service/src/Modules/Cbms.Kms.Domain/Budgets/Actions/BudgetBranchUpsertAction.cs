using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetBranchUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public int BranchId { get; private set; }
        public decimal AllocateAmount { get; private set; }

        public BudgetBranchUpsertAction (int id, int branchId, decimal allocateAmount)
        {
            Id = id;
            BranchId = branchId;
            AllocateAmount = allocateAmount;
        }
    }
}
