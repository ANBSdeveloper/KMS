using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(BudgetArea))]
    public class BudgetAreaDto : AuditedEntityDto
    {
        public int AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int SalesOrgId { get; set; }
        public int ParentSalesOrgId { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal TempRemainAmount { get; set; }
        public decimal TempUsedAmount { get; set; }
    }
}