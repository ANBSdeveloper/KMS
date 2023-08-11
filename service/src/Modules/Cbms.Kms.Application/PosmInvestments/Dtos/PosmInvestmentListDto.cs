using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmInvestments;

namespace Cbms.Kms.Application.PosmInvestments.Dto
{
    [AutoMap(typeof(PosmInvestment))]
    public class PosmInvestmentListDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public int RegisterStaffId { get; set; }
        public string RegisterStaffName { get; set; }
        public int BudgetId { get; set; }
        public int CustomerLocationId { get; set; }
        public decimal CurrentSalesAmount { get; set; }
        public decimal InvestmentAmount { get; set; }
        public int CycleId { get; set; }
        public PosmInvestmentStatus Status { get; set; }
        public string AreaName { get; set; }
        public string ZoneName { get; set; }

    }
}