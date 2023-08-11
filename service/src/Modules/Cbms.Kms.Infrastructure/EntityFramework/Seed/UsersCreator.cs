using Cbms.Authentication;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class UsersCreator
    {
        private readonly AppDbContext _context;

        public UsersCreator(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync()
        {
            var adminUser = _context.Users.FirstOrDefault(p => p.UserName == CbmsConsts.AdminUserName);
            var adminRole = _context.Roles.FirstOrDefault(p => p.RoleName == CbmsConsts.AdminRole);
            if (adminUser == null)
            {
                adminUser = new User();
                await adminUser.ApplyActionAsync(new CreateUserAction(CbmsConsts.AdminUserName, CbmsConsts.AdminUserName, PasswordManager.HashPassword("123456"), "", "", null, null, true));

                var userRole = new UserRole();
                await userRole.UpsertAsync(new UpsertUserRoleAction(adminRole.Id));

                await adminUser.ApplyActionAsync(new CrudRoleToUserAction(
                    new System.Collections.Generic.List<UserRole>{
                        userRole
                    },
                    new System.Collections.Generic.List<UserRole>())
                );

                await _context.Users.AddAsync(adminUser);
            }

            await _context.OrignalSaveAsync();
        }
    }
}