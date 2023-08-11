using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Budgets.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Budgets
{
    public class BudgetArea : AuditedEntity
    {
        public int AreaId { get; private set; }
        public int BudgetId { get; private set; }
        public decimal AllocateAmount { get; private set; }
        public decimal TempUsedAmount { get; private set; }
        public decimal TempRemainAmount { get; private set; }
        public decimal UsedAmount { get; private set; }
        public decimal RemainAmount { get; private set; }
        private BudgetArea()
        {
        }

        public static BudgetArea Create()
        {
            return new BudgetArea();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case BudgetAreaUpsertAction  upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case BudgetAreaTemporaryUseAction temporaryUseAction:
                    await TemporaryUseAsync(temporaryUseAction);
                    break;
                case BudgetAreaUseAction useAction:
                    await UseAsync(useAction);
                    break;
            }
        }

        public async Task UpsertAsync(BudgetAreaUpsertAction  action)
        {
            AreaId = action.AreaId;
            AllocateAmount = action.AllocateAmount;
            RemainAmount = action.AllocateAmount - UsedAmount;
            TempRemainAmount = action.AllocateAmount - TempUsedAmount;
        }

        public async Task TemporaryUseAsync(BudgetAreaTemporaryUseAction action)
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

        public async Task UseAsync(BudgetAreaUseAction action)
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