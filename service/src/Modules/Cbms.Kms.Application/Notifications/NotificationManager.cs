using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Notifications.Actions;
using Cbms.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications
{
    public class NotificationManager : INotificationManager, ITransientDependency
    {
        private readonly INotificationSender _sender;
        private readonly IIocResolver _iocResolver;
        private readonly IRepository<Notification, int> _notificationRepository;
        private readonly ILocalizationManager _localizationManager;
        public NotificationManager(
            INotificationSender sender,
            IRepository<Notification, int> notificationRepository,
            IIocResolver iocResolver,
            ILocalizationManager localizationManager
        )
        {
            _iocResolver = iocResolver;
            _sender = sender;
            _notificationRepository = notificationRepository;
            _localizationManager = localizationManager;
    }

        public async Task CreateAndSendSync(NotificationObjectType type, string subject, string shortContent, string content, List<int> userIds)
        {
            var notification = new Notification();
            await notification.ApplyActionAsync(new NotificationUpsertAction(
                true,
                type,
                subject,
                shortContent,
                content,
                true,
                new List<int>(),
                new List<int>()
                )
            );

            await notification.ApplyActionAsync(new NotificationCreateDetailFromUserAction(_iocResolver, userIds));

            await _notificationRepository.InsertAsync(notification);

            await notification.ApplyActionAsync(new NotificationSendAction(_iocResolver, _localizationManager.GetDefaultSource()));
        }
    }
}