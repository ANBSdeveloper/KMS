using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.RewardPackages.Actions
{
    public class RewardBranchUpsertAction : IEntityAction
    {
        public int Id { get; private set; }
        public int BranchId { get; private set; }

        public RewardBranchUpsertAction(
            int id,
            int branchId)
        {
            Id = id;
            BranchId = branchId;
        }
    }
}