using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public interface INotificationManager
    {
        public Task CreateAndSendSync(NotificationObjectType type, string subject, string shortContent, string content, List<int> userId);
    }
}
