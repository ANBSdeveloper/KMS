using Cbms.Kms.Domain.InvestmentSettings;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class InvestmentSettingsCreator
    {
        private readonly AppDbContext _context;

        public InvestmentSettingsCreator(AppDbContext context)
        {
            _context = context;
        }

        private async Task AddSettingAsync()
        {
            var setting = _context.InvestmentSetting.FirstOrDefault();
            if (setting == null)
            {
                setting = new InvestmentSetting();
                await setting.ApplyActionAsync(new UpsertInvestmentSettingAction(0, 0, 12, false, 0, 7, 2));
                await _context.InvestmentSetting.AddAsync(setting);
            }
        }

        public async Task CreateAsync()
        {
            await AddSettingAsync();
        }
    }
}