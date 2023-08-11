using Cbms.Authorization.Users;
using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Infrastructure;
using Cbms.Runtime.Connection;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Staffs
{
    public class StaffUserFinder : IStaffUserFinder, ITransientDependency
    {
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly AppDbContext _dbContext;

        public StaffUserFinder(
            AppDbContext dbContext, 
            IRepository<Staff, int> staffRepository, 
            ISqlConnectionFactory sqlConnectionFactory,
            IRepository<Customer, int> customerRepository)
        {
            _dbContext = dbContext;
            _staffRepository = staffRepository;
            _sqlConnectionFactory = sqlConnectionFactory;
            _customerRepository = customerRepository;
        }

        public async Task<Staff> FindAsmOfStaff(int staffId)
        {
            var staff = await _staffRepository.GetAsync(staffId);
            if (staff.StaffTypeCode == KmsConsts.AsmRole) return staff;

            var asmStaff = FindParentStaff(staff.SalesOrgId, KmsConsts.AsmRole);

            return asmStaff;
        }

        public async Task<Staff> FindRsmOfStaff(int staffId)
        {
            var staff = await _staffRepository.GetAsync(staffId);
            if (staff.StaffTypeCode == KmsConsts.RsmRole) return staff;

            var rsmStaff = FindParentStaff(staff.SalesOrgId, KmsConsts.RsmRole);

            return rsmStaff;
        }

        private Staff FindParentStaff(int salesOrgId, string role)
        {
            var parentStaff = (from st in _dbContext.Staffs
                               join parentOrg in _dbContext.SalesOrgs on st.SalesOrgId equals parentOrg.Id
                               join childOrg in _dbContext.SalesOrgs on parentOrg.Id equals childOrg.ParentId
                               where childOrg.Id == salesOrgId && st.IsActive
                               select st).FirstOrDefault();

            if (parentStaff != null)
            {
                if (parentStaff.StaffTypeCode == role)
                {
                    return parentStaff;
                }
                else
                {
                    return FindParentStaff(parentStaff.SalesOrgId, role);
                }
            }
            return null;
        }

        public async Task<List<Staff>> FindByBranchAsync(List<int> branchIds)
        {
            var staffs = new List<Staff>();
            staffs.AddRange(await GetStaffsInBranchesByRoleAsync(branchIds, KmsConsts.RsmRole));
            staffs.AddRange(await GetStaffsInBranchesByRoleAsync(branchIds, KmsConsts.AsmRole));
            staffs.AddRange(await GetStaffsInBranchesByRoleAsync(branchIds, KmsConsts.SalesSupervisorRole));
            return staffs;
        }

        private async Task<IEnumerable<Staff>> GetStaffsInBranchesByRoleAsync(List<int> branchIds, string role)
        {
            string sql = $@"
                        WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN Branches AS b ON SalesOrgs.Id = b.SalesOrgId
                            WHERE b.Id IN @BranchIds

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.Id = CTE.ParentId
                        )

                        SELECT s.*
                        FROM Staffs AS s
                        WHERE s.SalesOrgId IN (
	                        SELECT CTE.Id
	                        FROM CTE
                        ) AND s.StaffTypeCode = '{role}' AND s.IsActive = 1 ";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            return await connection.QueryAsync<Staff>(sql, new {
                branchIds
            });
        }

        public Task<Staff> FindById(int staffId)
        {
            return _staffRepository.FindAsync(staffId);
        }

        public async Task<User> FindUserManageCustomer(int customerId, string roleName)
        {
            var customer = await _customerRepository.FindAsync(customerId);
            string sql = $@"
                        WITH CTE AS
                        (
                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN Branches AS b ON SalesOrgs.Id = b.SalesOrgId
                            WHERE b.Id = @BranchId

                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE ON SalesOrgs.Id = CTE.ParentId
                        )
                        SELECT s.*
                        FROM Users AS s
                        WHERE EXISTS (
	                        SELECT TOP 1 *
	                        FROM CTE
	                        INNER JOIN UserAssignments AS assignment ON  CTE.Id = assignment.SalesOrgId 
	                        WHERE assignment.UserId = s.Id
                        ) AND s.IsActive = 1 
                        AND EXISTS (
	                        SELECT TOP 1 *
	                        FROM UserRoles AS userRole
	                        INNER JOIN Roles AS role ON userRole.RoleId = role.Id
	                        WHERE userRole.UserId = s.Id AND RoleName = @RoleName
                        )";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                branchId = customer.BranchId,
                roleName
            });

        }

        public async Task<List<User>> FindUsersManageCustomer(int customerId, params string[] roleNames)
        {
            List<User> users = new List<User>();
            foreach (var item in roleNames)
            {
                var user = await FindUserManageCustomer(customerId, item);
                if (user!=null)
                {
                    users.Add(user);
                }
            }
            return users;
        }
    }
}