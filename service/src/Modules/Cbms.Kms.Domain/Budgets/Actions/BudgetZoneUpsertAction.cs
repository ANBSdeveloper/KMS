using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetZoneUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public int ZoneId { get; private set; }
        public decimal AllocateAmount { get; private set; }

        public BudgetZoneUpsertAction (int id, int zoneId, decimal allocateAmount)
        {
            Id = id;
            ZoneId = zoneId;
            AllocateAmount = allocateAmount;
        }
    }
}
