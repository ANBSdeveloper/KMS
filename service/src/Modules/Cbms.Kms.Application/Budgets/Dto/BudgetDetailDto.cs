using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(BudgetDetail))]
    public class BudgetDetailDto : AuditedEntityDto
    {
        public int StaffId { get; set; }
        public int CreditPoint { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public string RoleName { get; set; }
        public int? UserId { get; set; }
        public int SalesOrgId { get; set; }
        public int ParentSalesOrgId { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal TempRemainAmount { get; set; }
        public decimal TempUsedAmount { get; set; }
    }
}