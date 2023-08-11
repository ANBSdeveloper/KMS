using System.Threading.Tasks;

namespace Cbms.Kms.Domain.AppSettings
{
    public interface IAppSettingManager
    {
        Task InsertOrUpdateAsync(string code, string value, string description);
        Task<string> GetAsync(string code);
        Task<bool> IsEnableAsync(string code);
    }
}
