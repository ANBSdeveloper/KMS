using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public interface ITicketInvestmentManager
    {
        Task<string> GenerateCodeAsync(int customerId);
        Task<Ticket> GenerateTicketAsync(int ticketInvestment, string consumerPhone, string consumerName);
        Task<TicketInvestment> GetActiveTicketInvestmentAsync(int userId, DateTime validDate);
        Task<string> GetHistoryDataAsync(TicketInvestment ticketInvestment);
    }
}
