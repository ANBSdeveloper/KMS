using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.InvestmentBranchSettings
{
    public class InvestmentBranchSetting : AuditedAggregateRoot
    {
        public bool IsEditablePoint { get; private set; }
        public int BranchId { get; private set; }
        public InvestmentBranchSetting()
        {
        }

        public static InvestmentBranchSetting Create()
        {
            return new InvestmentBranchSetting();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case InvestmentBranchSettingUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(InvestmentBranchSettingUpsertAction action)
        {
            IsEditablePoint = action.IsEditablePoint;
            BranchId = action.BranchId;
        }
    }
}
