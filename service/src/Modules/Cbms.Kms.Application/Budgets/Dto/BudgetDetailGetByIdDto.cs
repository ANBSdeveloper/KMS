using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Budgets.Dto
{
    public class BudgetDetailGetByIdDto: BudgetDetailBase
    {        
        public List<TicketInvestmentHistoryByBudgetIdDto> TicketInvestmentHistory { get; set; }
        
    }
}