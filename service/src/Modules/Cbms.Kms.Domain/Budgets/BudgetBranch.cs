using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Budgets.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Budgets
{
    public class BudgetBranch : AuditedEntity
    {
        public int BranchId { get; private set; }
        public int BudgetId { get; private set; }
        public decimal AllocateAmount { get; private set; }
        public decimal TempUsedAmount { get; private set; }
        public decimal TempRemainAmount { get; private set; }
        public decimal UsedAmount { get; private set; }
        public decimal RemainAmount { get; private set; }
        private BudgetBranch()
        {
        }

        public static BudgetBranch Create()
        {
            return new BudgetBranch();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case BudgetBranchUpsertAction  upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case BudgetBranchTemporaryUseAction temporaryUseAction:
                    await TemporaryUseAsync(temporaryUseAction);
                    break;
                case BudgetBranchUseAction useAction:
                    await UseAsync(useAction);
                    break;
            }
        }

        public async Task UpsertAsync(BudgetBranchUpsertAction  action)
        {
            BranchId = action.BranchId;
            AllocateAmount = action.AllocateAmount;
            RemainAmount = action.AllocateAmount - UsedAmount;
            TempRemainAmount = action.AllocateAmount - TempUsedAmount;
        }

        public async Task TemporaryUseAsync(BudgetBranchTemporaryUseAction action)
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

        public async Task UseAsync(BudgetBranchUseAction action)
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