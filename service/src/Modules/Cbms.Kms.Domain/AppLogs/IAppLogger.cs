using System.Threading.Tasks;

namespace Cbms.Kms.Domain.AppLogs
{
    public interface IAppLogger
    {
        Task LogInfoAsync(string name, object data);
        Task LogInfoAsync(string name, string data);
        Task LogErrorAsync(string name, object data);
        Task LogErrorAsync(string name, string data);
    }
}
