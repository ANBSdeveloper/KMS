using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Timing;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public class NotificationUser : Entity, IHasCreationTime
    {
        public int UserId { get; private set; }
        public DateTime? ViewDate { get; private set; }
        public DateTime CreationTime { get; set; }
        public int NotificationId { get; private set; }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case NotificationUserCreateAction upsertAction:
                    await CreateAsync(upsertAction);
                    break;
                case NotificationUserViewAction viewAction:
                    await ViewAsync(viewAction);
                    break;
            }
        }

        private async Task CreateAsync(NotificationUserCreateAction action)
        {
            UserId = action.UserId;
            CreationTime = Clock.Now;
        }

        private async Task ViewAsync(NotificationUserViewAction action)
        {
            ViewDate = Clock.Now;
        }
    }
}
