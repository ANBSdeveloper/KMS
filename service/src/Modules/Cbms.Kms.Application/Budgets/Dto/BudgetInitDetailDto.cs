using System.Collections.Generic;

namespace Cbms.Kms.Application.Budgets.Dto
{
    public class BudgetInitDetailDto
    {
        public List<BudgetZoneDto> Zones { get; set; }
        public List<BudgetAreaDto> Areas { get; set; }
        public List<BudgetBranchDto> Branches { get; set; }
    }
}