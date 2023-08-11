using Cbms.Dependency;
using Cbms.Kms.Domain.Notifications;
using Hangfire;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications
{
    public class NotificationSender : INotificationSender, ITransientDependency
    {
        public async Task SendSync(int notificationId)
        {
            var jobId = BackgroundJob.Schedule<NotificationJob>(c => c.RunAsync(notificationId), TimeSpan.FromMinutes(1));
        }
    }
}