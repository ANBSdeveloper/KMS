using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.InvestmentSettings;

namespace Cbms.Kms.Application.InvestmentSettings.Dto
{
    [AutoMap(typeof(InvestmentSetting))]
    public class InvestmentSettingBaseDto : AuditedEntityDto
    {
        public decimal MaxInvestAmount { get; set; }
        public decimal AmountPerPoint { get; set; }
        public decimal MaxInvestmentQueryMonths { get; set; }
        public decimal DefaultPointsForTicket { get; set; }
        public bool CheckQrCodeBranch { get; set; }
        public int BeginIssueDaysAfterCurrent { get; set; }
        public int EndIssueDaysBeforeOperation { get; set; }
    }
}