using Cbms.Application.Runtime.DistributedLock;
using Cbms.Kms.Domain.Notifications;
using Hangfire;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications
{
    public class SmsMessageJob
    {
        private readonly ISmsMessageSender _smsSender;
        private readonly DistributedLockManager _distributedLockManager;

        public SmsMessageJob(ISmsMessageSender smsSender, DistributedLockManager distributedLockManager)
        {
            _smsSender = smsSender;
            _distributedLockManager = distributedLockManager;
        }

        [Queue("notification")]
        public async Task RunAsync(string phone, string content)
        {
            await using (await _distributedLockManager.AcquireAsync($"sms_send_{phone}"))
            {
                await _smsSender.SendAsync(phone, content);
            }
        }
    }
}