using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Notifications.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public class NotificationBranch : AuditedEntity
    {
        public int BranchId { get; private set; }
        public int NotificationId { get; private set; }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case NotificationBranchUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(NotificationBranchUpsertAction action)
        {
            BranchId = action.BranchId;
        }
    }
}
