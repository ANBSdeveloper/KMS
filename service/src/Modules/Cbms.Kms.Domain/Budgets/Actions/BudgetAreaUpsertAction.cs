using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetAreaUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public int AreaId { get; private set; }
        public decimal AllocateAmount { get; private set; }

        public BudgetAreaUpsertAction (int id, int areaId, decimal allocateAmount)
        {
            Id = id;
            AreaId = areaId;
            AllocateAmount = allocateAmount;
        }
    }
}
