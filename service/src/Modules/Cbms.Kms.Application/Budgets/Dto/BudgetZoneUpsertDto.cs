using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(BudgetZone))]
    public class BudgetZoneUpsertDto : EntityDto
    {
        public int ZoneId { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public int SalesOrgId { get; set; }
        public int ParentSalesOrgId { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
    }
}