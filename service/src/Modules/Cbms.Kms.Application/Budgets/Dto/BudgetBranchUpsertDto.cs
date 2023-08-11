using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(BudgetBranch))]
    public class BudgetBranchUpsertDto : EntityDto
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int SalesOrgId { get; set; }
        public int ParentSalesOrgId { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal UsedAmount { get; set; }
    }
}