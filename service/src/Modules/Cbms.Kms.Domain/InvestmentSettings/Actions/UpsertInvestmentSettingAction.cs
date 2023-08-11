using Cbms.Domain.Entities;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Domain.InvestmentSettings
{
    public class UpsertInvestmentSettingAction : IEntityAction
    {
        public decimal MaxInvestAmount { get; private set; }
        public decimal AmountPerPoint { get; private set; }
        public decimal MaxInvestmentQueryMonths { get; private set; }
        public bool CheckQrCodeBranch { get; private set; }
        public decimal DefaultPointsForTicket { get; private set; }
        public int BeginIssueDaysAfterCurrent { get; private set; }
        public int EndIssueDaysBeforeOperation { get; private set; }

        public UpsertInvestmentSettingAction(decimal maxInvestAmount, decimal amountPerPoint, decimal maxInvestmentQueryMonths, 
            bool checkQrCodeBranch, decimal defaultPointsForTicket, int beginIssueDaysAfterCurrent, int endIssueDaysBeforeOperation)
        {
            MaxInvestAmount = maxInvestAmount;
            AmountPerPoint = amountPerPoint;
            MaxInvestmentQueryMonths = maxInvestmentQueryMonths;
            CheckQrCodeBranch = checkQrCodeBranch;
            DefaultPointsForTicket = defaultPointsForTicket;
            BeginIssueDaysAfterCurrent = beginIssueDaysAfterCurrent;
            EndIssueDaysBeforeOperation = endIssueDaysBeforeOperation;
        }
    }
}
