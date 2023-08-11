using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Budgets.Actions;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Cycles;
using Cbms.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Budgets
{
    public class BudgetManager : IBudgetManager, ITransientDependency
    {
        private readonly IRepository<Budget, int> _budgetRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Cycle, int> _cycleRepository;
        private readonly IIocResolver _iocResolver;
        private readonly ILocalizationManager _localizationManager;
        public BudgetManager(
            IIocResolver iocResolver,
            ILocalizationManager localizationManager,
            IRepository<Budget, int> budgetRepository,
            IRepository<Customer,int> customerRepository,
            IRepository<Cycle, int> cycleRepository)
        {
            _budgetRepository = budgetRepository;
            _cycleRepository = cycleRepository;
            _customerRepository = customerRepository;
            _iocResolver = iocResolver;
            _localizationManager = localizationManager;
        }

        public async Task<Budget> TemporaryUseAsync(BudgetInvestmentType type, int customerId, DateTime useDate, decimal amount)
        {
            var cycle = _cycleRepository
                .GetAll()
                .FirstOrDefault(p => p.FromDate <= useDate && p.ToDate >= useDate);
            var localizationSource = _localizationManager.GetDefaultSource();
            if (cycle == null)
            {
                throw BusinessExceptionBuilder.Create(localizationSource)
                    .MessageCode("PosmInvestment.CycleNotFound", useDate.ToString())
                    .Build();
            }
            var budget = _budgetRepository
                .GetAllIncluding(p => p.Zones, p => p.Areas, p => p.Branches)
                .FirstOrDefault(p => p.InvestmentType == type && p.CycleId == cycle.Id);

            
            if (budget == null)
            {
                throw BusinessExceptionBuilder.Create(localizationSource)
                    .MessageCode("Budget.NotValidForInvestment", cycle.Number)
                    .Build();
            }

            var customer = await _customerRepository.GetAsync(customerId);
            if (customer.BranchId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetTemporaryUseAction(
                    _iocResolver,
                    localizationSource,
                    BudgetLevelType.Branch,
                    customer.BranchId.Value,
                    amount
                ));
            }
            else if (customer.AreaId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetTemporaryUseAction(
                   _iocResolver,
                   localizationSource,
                   BudgetLevelType.Area,
                   customer.AreaId.Value,
                   amount
               ));
            }
            else if (customer.ZoneId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetTemporaryUseAction(
                   _iocResolver,
                   localizationSource,
                   BudgetLevelType.Zone,
                   customer.ZoneId.Value,
                   amount
               ));
            }
            await _budgetRepository.UnitOfWork.CommitAsync();
            return budget;
        }

        public async Task<Budget> UseAsync(BudgetInvestmentType type, int customerId, DateTime useDate, decimal temporaryAmount, decimal amount)
        {
            var cycle = _cycleRepository.GetAll().FirstOrDefault(p => p.FromDate <= useDate && p.ToDate >= useDate);
            var budget = _budgetRepository
              .GetAllIncluding(p => p.Zones, p => p.Areas, p => p.Branches)
              .FirstOrDefault(p => p.InvestmentType == type && p.CycleId == cycle.Id);

            var localizationSource = _localizationManager.GetDefaultSource();
            if (budget == null)
            {
                throw BusinessExceptionBuilder.Create(localizationSource)
                    .MessageCode("Budget.NotValidForInvestment", cycle.Number)
                    .Build();
            }
            var customer = await _customerRepository.GetAsync(customerId);
            if (customer.BranchId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetUseAction(
                    _iocResolver,
                    localizationSource,
                    BudgetLevelType.Branch,
                    customer.BranchId.Value,
                    temporaryAmount,
                    amount
                ));
            }
            else if (customer.AreaId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetUseAction(
                    _iocResolver,
                    localizationSource,
                    BudgetLevelType.Area,
                    customer.AreaId.Value,
                    temporaryAmount,
                    amount
               ));
            }
            else if (customer.ZoneId.HasValue)
            {
                await budget.ApplyActionAsync(new BudgetUseAction(
                    _iocResolver,
                    localizationSource,
                    BudgetLevelType.Zone,
                    customer.ZoneId.Value,
                    temporaryAmount,
                    amount
               ));
            }

            await _budgetRepository.UnitOfWork.CommitAsync();

            return budget;
        }
    }
}
