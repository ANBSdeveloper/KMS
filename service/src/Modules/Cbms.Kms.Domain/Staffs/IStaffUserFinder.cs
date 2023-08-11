using Cbms.Authorization.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Staffs
{
    public interface IStaffUserFinder
    {
        Task<Staff> FindById(int staffId);
        Task<Staff> FindAsmOfStaff(int staffId);
        Task<Staff> FindRsmOfStaff(int staffId);
        Task<User> FindUserManageCustomer(int customerId, string roleName);
        Task<List<User>> FindUsersManageCustomer(int customerId,params string[] roleName);
        Task<List<Staff>> FindByBranchAsync(List<int> branchIds);
    }
}
