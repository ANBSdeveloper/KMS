using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(Budget))]
    public class BudgetListItemDto : AuditedEntityDto
    {
        public int CycleId { get; set; }
        public string CycleNumber { get; set; }
        public int InvestmentType { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal AllocateAmount { get;set; }
        public decimal TempRemainAmount { get; set; }
        public decimal TempUsedAmount { get; set; }
    }
}