using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.InvestmentBranchSettings
{
    public class InvestmentBranchSettingUpsertAction : IEntityAction
    {
        public int BranchId { get; private set; }
        public bool IsEditablePoint { get; private set; }

        public InvestmentBranchSettingUpsertAction(int branchId, bool isEditablePoint)
        {
            IsEditablePoint = isEditablePoint;
            BranchId = branchId;
        }
    }
}
