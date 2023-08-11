using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Budgets.Commands;
using Cbms.Kms.Domain.Budgets;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets.CommandHandlers
{
    public class BudgetDeleteCommandHandler : DeleteEntityCommandHandler<BudgetDeleteCommand, Budget>
    {
        private readonly IRepository<Budget, int> _budgetRepository;
        public BudgetDeleteCommandHandler(IRequestSupplement supplement, IRepository<Budget, int> budgetRepository) : base(supplement)
        {
            _budgetRepository = budgetRepository;
        }

        public async override Task<Unit> Handle(BudgetDeleteCommand request, CancellationToken cancellationToken)
        {
            var budget = _budgetRepository.GetAllIncluding(x => x.Zones, x => x.Areas, x => x.Branches).FirstOrDefault(p => p.Id == request.Id);

            if (budget != null && budget.Zones.Where(p=>p.UsedAmount >0).Count() > 0)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Budget.UsingCantDelete").Build();
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}