using Cbms.Kms.Domain.TicketInvestments;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmInvestments
{
    public interface IPosmInvestmentManager
    {
        Task<string> GenerateCodeAsync(int customerId);
        //Task<TicketInvestment> GetActiveTicketInvestmentAsync(int userId, DateTime validDate);
        Task<string> GetHistoryDataAsync(PosmInvestmentItem item);
        Task<decimal> GetPriceAsync(int posmItemId);
    }
}
