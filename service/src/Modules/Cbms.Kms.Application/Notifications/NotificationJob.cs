using Cbms.Application.Runtime.DistributedLock;
using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Hangfire;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications
{
    public class NotificationJob
    {
        private readonly IIocResolver _iocResolver;
        private readonly IRepository<Notification, int> _notificationRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public NotificationJob(IIocResolver iocResolver, IRepository<Notification, int> notificationRepository, DistributedLockManager distributedLockManager)
        {
            _iocResolver = iocResolver;
            _notificationRepository = notificationRepository;
            _distributedLockManager = distributedLockManager;
        }

        [Queue("notification")]
        public async Task RunAsync(int notficationId)
        {
            await using (await _distributedLockManager.AcquireAsync($"notification_send_{notficationId}"))
            {
                var notification = _notificationRepository
                    .GetAllIncluding(p => p.NotificationBranches)
                    .FirstOrDefault(p => p.Id == notficationId);

                await notification.ApplyActionAsync(new NotificationGenerateUserAction(_iocResolver));
                await notification.ApplyActionAsync(new NotificationCompleteAction(_iocResolver));

                await _notificationRepository.UnitOfWork.CommitAsync();
            }
        }
    }
}