using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Customers
{
    public interface ICustomerManager
    {
        Task<bool> IsManageByUserAsync(int userId, int customerId);
        Task<decimal> GetActualSalesAmountAsync(int customerId, DateTime fromDate, DateTime toDate);
        Task ScheduleCalculateEfficientAsync(int customerId);
    }
}
