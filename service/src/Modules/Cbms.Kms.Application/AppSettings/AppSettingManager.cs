using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.AppSettings.Actions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.AppSettings
{
    public class AppSettingManager : IAppSettingManager, ITransientDependency
    {
        private IRepository<AppSetting, int> _repository;

        public AppSettingManager(IRepository<AppSetting, int> repository)
        {
            _repository = repository;
        }

        public async Task<string> GetAsync(string code)
        {
            var setting = await _repository.GetAll().FirstOrDefaultAsync(p => p.Code == code);
            return setting?.Value;
        }

        public async Task<bool> IsEnableAsync(string code)
        {
            var setting = await _repository.GetAll().FirstOrDefaultAsync(p => p.Code == code);
            return (setting?.Value ?? "") == "1";
        }

        public async Task InsertOrUpdateAsync(string code, string value, string description)
        {
            var setting = await _repository.GetAll().FirstOrDefaultAsync(p => p.Code == code);
            if (setting == null)
            {
                setting = new AppSetting();
                await _repository.InsertAsync(setting);
            }

            await setting.ApplyActionAsync(new AppSettingUpsertAction(code, value, description));
            await _repository.UnitOfWork.CommitAsync();
        }

      
    }
}