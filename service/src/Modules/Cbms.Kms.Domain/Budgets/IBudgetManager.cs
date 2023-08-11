using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Budgets
{
    public interface IBudgetManager
    {
        Task<Budget> TemporaryUseAsync(BudgetInvestmentType type, int staffId, DateTime useDate, decimal amount);
        Task<Budget> UseAsync(BudgetInvestmentType type, int staffId, DateTime useDate, decimal temporaryAmount, decimal amount);
    }
}
