using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Budgets.Actions;
using Cbms.Kms.Domain.Staffs;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Budgets
{
    public class BudgetZone : AuditedEntity
    {
        public int ZoneId { get; private set; }
        public int BudgetId { get; private set; }
        public decimal AllocateAmount { get; private set; }
        public decimal TempUsedAmount { get; private set; }
        public decimal TempRemainAmount { get; private set; }
        public decimal UsedAmount { get; private set; }
        public decimal RemainAmount { get; private set; }
        private BudgetZone()
        {
        }

        public static BudgetZone Create()
        {
            return new BudgetZone();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case BudgetZoneUpsertAction  upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case BudgetZoneTemporaryUseAction temporaryUseAction:
                    await TemporaryUseAsync(temporaryUseAction);
                    break;
                case BudgetZoneUseAction useAction:
                    await UseAsync(useAction);
                    break;
            }
        }

        public async Task UpsertAsync(BudgetZoneUpsertAction  action)
        {
            ZoneId = action.ZoneId;
            AllocateAmount = action.AllocateAmount;
            RemainAmount = action.AllocateAmount - UsedAmount;
            TempRemainAmount = action.AllocateAmount - TempUsedAmount;
        }

        public async Task TemporaryUseAsync(BudgetZoneTemporaryUseAction action)
        {
            if (TempRemainAmount < action.UseAmount)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                   .MessageCode("Budget.OverTemp", action.UseAmount.ToString("N0"), TempRemainAmount.ToString("N0"))
                   .Build();
            }

            TempUsedAmount = TempUsedAmount + action.UseAmount;
            TempRemainAmount = AllocateAmount - TempUsedAmount;
        }

        public async Task UseAsync(BudgetZoneUseAction action)
        {
            if (RemainAmount < action.UseAmount)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                   .MessageCode("Budget.OverTemp", action.UseAmount.ToString("N0"), TempRemainAmount.ToString("N0"))
                   .Build();
            }

            TempUsedAmount = TempUsedAmount - action.UseTemporaryAmount + action.UseAmount;
            TempRemainAmount = AllocateAmount - TempUsedAmount;

            UsedAmount = UsedAmount + action.UseAmount;
            RemainAmount = AllocateAmount - UsedAmount;
        }
    }
}