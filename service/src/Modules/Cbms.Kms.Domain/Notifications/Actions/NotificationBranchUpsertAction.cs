using Cbms.Domain.Entities;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationBranchUpsertAction : IEntityAction
    {
        public int BranchId { get; private set; }
        public NotificationBranchUpsertAction(int branchId)
        {
            BranchId = branchId;
        }
    }
}
