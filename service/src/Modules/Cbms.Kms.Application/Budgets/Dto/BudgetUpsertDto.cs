using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;

namespace Cbms.Kms.Application.Budgets.Dto
{
    public class BudgetUpsertDto : EntityDto
    {
        public int CycleId { get; set; }
        public BudgetInvestmentType InvestmentType { get; set; }
        public CrudListDto<BudgetZoneUpsertDto> ZonesChanges { get; set; }
        public CrudListDto<BudgetAreaUpsertDto> AreasChanges { get; set; }
        public CrudListDto<BudgetBranchUpsertDto> BranchesChanges { get; set; }
        public BudgetUpsertDto()
        {
            ZonesChanges = new CrudListDto<BudgetZoneUpsertDto>();
            AreasChanges = new CrudListDto<BudgetAreaUpsertDto>();
            BranchesChanges = new CrudListDto<BudgetBranchUpsertDto>();
        }
    }
}