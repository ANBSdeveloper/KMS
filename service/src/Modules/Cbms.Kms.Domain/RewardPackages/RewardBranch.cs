using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.RewardPackages.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.RewardPackages
{
    public class RewardBranch : AuditedAggregateRoot
    {
        public int RewardPackageId { get; private set; }
        public int BranchId { get; private set; }

        public RewardBranch()
        {
        }

        public static RewardBranch Create()
        {
            return new RewardBranch();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case RewardBranchUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(RewardBranchUpsertAction action)
        {
            BranchId = action.BranchId;
        }
    }
}