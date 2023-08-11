using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.InvestmentSettings
{
    public class InvestmentSetting : AuditedAggregateRoot
    {
        public decimal MaxInvestAmount { get; private set; }
        public decimal AmountPerPoint { get; private set; }
        public decimal MaxInvestmentQueryMonths { get; private set; }
        public bool CheckQrCodeBranch { get; private set; }
        public decimal DefaultPointsForTicket { get; private set; }
        public int BeginIssueDaysAfterCurrent { get; private set; }
        public int EndIssueDaysBeforeOperation { get; private set; }
        public InvestmentSetting()
        {
        }

        public static InvestmentSetting Create()
        {
            return new InvestmentSetting();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertInvestmentSettingAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertInvestmentSettingAction action)
        {
            MaxInvestAmount = action.MaxInvestAmount;
            AmountPerPoint = action.AmountPerPoint;
            MaxInvestmentQueryMonths = action.MaxInvestmentQueryMonths;
            CheckQrCodeBranch = action.CheckQrCodeBranch;
            DefaultPointsForTicket = action.DefaultPointsForTicket;
            BeginIssueDaysAfterCurrent = action.BeginIssueDaysAfterCurrent;
            EndIssueDaysBeforeOperation = action.EndIssueDaysBeforeOperation;
        }
    }
}
