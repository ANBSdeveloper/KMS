using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Notifications
{
    public interface ISmsMessageSender
    {
        Task ScheduleAsync(string phone, string content);
        Task<bool> SendAsync(string phone, string content);
    }
}
