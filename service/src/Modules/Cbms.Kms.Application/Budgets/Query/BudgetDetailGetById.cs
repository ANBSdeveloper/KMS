using Cbms.Kms.Application.Budgets.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Budgets.Query
{
    public class BudgetDetailGetById : QueryBase, IRequest<BudgetDetailGetByIdDto>
    {
        public BudgetDetailGetById(int cycleId, int staffId, int investmentType)
        {
            CycleId = cycleId;
            StaffId = staffId;
            InvestmentType = investmentType;
        }

        public int CycleId { get; set; }
        public int StaffId { get; set; }
        public int InvestmentType { get; set; }
    }
}