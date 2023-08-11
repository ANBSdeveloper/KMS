using Cbms.Authorization.Roles;
using Cbms.Authorization.Roles.Actions;
using Cbms.Kms.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class RolesCreator
    {
        private readonly AppDbContext _context;

        public RolesCreator(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync()
        {
            await AddRoleAsync(KmsConsts.AdminRole, "System Administrator");
            await AddRoleAsync(KmsConsts.SalesSupervisorRole, "Sales Supervisor");
            await AddRoleAsync(KmsConsts.AsmRole, "Area Sales Manager");
            await AddRoleAsync(KmsConsts.RsmRole, "Region Sales Manager");
            await AddRoleAsync(KmsConsts.TradeAdminRole, "Trade Admin");
            await AddRoleAsync(KmsConsts.TradeMarketingRole, "Trade Marketing");
            await AddRoleAsync(KmsConsts.SalesAdminRole, "Sales Administrator");
            await AddRoleAsync(KmsConsts.SalesDirector, "Sales Director");
            await AddRoleAsync(KmsConsts.DevelopmentDirector, "Business Development Director");
            await AddRoleAsync(KmsConsts.CustomerDevelopmentManagerRole, "Customer Development Manager");
            await AddRoleAsync(KmsConsts.CustomerDevelopmentLeadRole, "Customer Development Lead ");
            await AddRoleAsync(KmsConsts.CustomerDevelopmentAdminRole, "Customer Development Admin");
            await AddRoleAsync("SR", "Sales Representative");
            await AddRoleAsync("PG", "PG");
            await AddRoleAsync(KmsConsts.ShopRole, "Shop");
            await AddRoleAsync(KmsConsts.SupplyRole, "Supply");
            await AddRoleAsync(KmsConsts.MarketingRole, "Marketing");
            await _context.OrignalSaveAsync();
        }

        private async Task AddRoleAsync(string code, string name)
        {
            var role = _context.Roles.FirstOrDefault(p => p.RoleName.ToUpper() == code.ToUpper());
            if (role == null)
            {
                role = new Role();
                await role.ApplyActionAsync(new UpsertRoleAction(code, name, "", true));
                await _context.Roles.AddAsync(role);
            }
        }
    }
}