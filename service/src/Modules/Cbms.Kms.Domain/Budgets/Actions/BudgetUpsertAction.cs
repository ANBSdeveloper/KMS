using Cbms.Domain.Entities;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.Budgets.Actions
{
    public class BudgetUpsertAction : IEntityAction
    {
        public int CycleId { get; private set; }
        public BudgetInvestmentType PortfolioType { get; private set; }
        public List<BudgetZoneUpsertAction> UpsertZones { get; set; }
        public List<int> DeleteZones { get; set; }
        public List<BudgetAreaUpsertAction> UpsertAreas { get; set; }
        public List<int> DeleteAreas { get; set; }
        public List<BudgetBranchUpsertAction> UpsertBranches { get; set; }
        public List<int> DeleteBranches { get; set; }
        public BudgetUpsertAction(
            int cycleId,
            BudgetInvestmentType type,
            List<BudgetZoneUpsertAction> upsertZones,
            List<int> deleteZones,
            List<BudgetAreaUpsertAction> upsertAreas,
            List<int> deleteAreas,
            List<BudgetBranchUpsertAction> upsertBranches,
            List<int> deleteBranches)
        {
            CycleId = cycleId;
            PortfolioType = type;
            UpsertZones = upsertZones;
            DeleteZones = deleteZones;
            UpsertAreas = upsertAreas;
            DeleteAreas = deleteAreas;
            UpsertBranches = upsertBranches;
            DeleteBranches = deleteBranches;
        }
    }
}