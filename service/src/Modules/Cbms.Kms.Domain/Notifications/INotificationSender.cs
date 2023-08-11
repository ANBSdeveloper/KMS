using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public interface INotificationSender
    {
        public Task SendSync(int notficationId);
    }
}
