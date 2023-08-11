using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Consumers
{
    public interface IConsumerFinder
    {
        Task<ConsumerInfo> FindByPhoneAsync(string phone);
        Task<ConsumerInfo> FindByPhoneInSalesForce(string phone);
    }
}
