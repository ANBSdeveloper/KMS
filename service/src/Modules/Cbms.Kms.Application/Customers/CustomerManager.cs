using Cbms.Authorization;
using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Extensions;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.OracleProvider;
using Cbms.Kms.Domain.Staffs;
using Cbms.Runtime.Connection;
using Dapper;
using Hangfire;
using System;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers
{
    public class CustomerManager : ICustomerManager, ITransientDependency
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly IOracleDataAccess _oracleDataAccess;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IUserManager _userManager;
        public CustomerManager(
            ISqlConnectionFactory sqlConnectionFactory,
            IRepository<Staff, int> staffRepository,
            IRepository<Customer, int> customerRepository,
            IUserManager userManager,
            IOracleDataAccess oracleDataAccess)
        {
            _userManager = userManager;
            _sqlConnectionFactory = sqlConnectionFactory;
            _staffRepository = staffRepository;
            _customerRepository = customerRepository;
            _oracleDataAccess = oracleDataAccess;
        }

        public async Task ScheduleCalculateEfficientAsync(int customerId)
        {
            BackgroundJob.Schedule<CalculateEfficientJob>(c => c.RunAsync(customerId), TimeSpan.FromSeconds(30));
        }

        public async Task<decimal> GetActualSalesAmountAsync(int customerId, DateTime fromDate, DateTime toDate)
        {
            var customer = await _customerRepository.GetAsync(customerId);

            var dataTable = _oracleDataAccess.ExecuteQuery($@"
                    SELECT
                        NVL(SUM(TOTAL),0) AS Amount
                    FROM VITADAIRY.SALE_ORDER so
                    JOIN VITADAIRY.CUSTOMER c ON so.CUSTOMER_ID = c.CUSTOMER_ID
                    WHERE so.APPROVED = 1 AND c.SHORT_CODE = '{customer.Code}'
                    AND so.ORDER_DATE >= to_date('{fromDate.BeginOfDay().ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                    AND so.ORDER_DATE <= to_date('{toDate.EndOfDay().ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                ");

            return dataTable.Rows[0].Field<decimal>("Amount");
        }

        public async Task<bool> IsManageByUserAsync(int userId, int customerId)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync(p => p.UserId == userId);
            var roles = await _userManager.GetRolesAsync(userId);
            if (roles.Select(p=>p.RoleName).Contains(KmsConsts.CustomerDevelopmentLeadRole))
            {
                staff = null;
            }

            string cteSql = "";
            if (staff != null)
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM   SalesOrgs
	                WHERE Id = {staff.SalesOrgId} ";
            }
            else
            {
                cteSql = $@"
                    SELECT SalesOrgs.*
                    FROM SalesOrgs
                    INNER JOIN UserAssignments ON SalesOrgs.Id = UserAssignments.SalesOrgId
	                WHERE UserAssignments.UserId = {userId} ";
            }

            string sql = $@"WITH CTE AS
                    (
                        {cteSql}

                        UNION ALL

                        SELECT SalesOrgs.*
                        FROM   SalesOrgs
                        INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                    )
                    SELECT c.*
                    FROM CTE
                    INNER JOIN Branches AS b ON b.SalesOrgId = cTE.Id
                    INNER JOIN  Customers AS c ON c.BranchId = b.Id
                    WHERE c.Id = {customerId} "
                    + (staff != null ? $@"AND (c.SalesSupervisorStaffId = {staff.Id} OR c.AsmStaffId = {staff.Id} OR c.RsmStaffId = {staff.Id}) " : "");

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var customer = await connection.QueryFirstOrDefaultAsync<CustomerDto>(sql);

            return customer != null;
        }
    }
}