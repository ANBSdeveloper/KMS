using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.InvestmentSettings.QueryHandler
{
    public class GetInvestmentCustomerSettingByCodeHandler : QueryHandlerBase, IRequestHandler<GetInvestmentCustomerSettingByCode, InvestmentCustomerSettingDto>
    {
        private readonly IRepository<InvestmentSetting, int> _settingRepository;
        private readonly IRepository<InvestmentBranchSetting, int> _branchSettingRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IAppSettingManager _appSettingManager;

        public GetInvestmentCustomerSettingByCodeHandler(
            IRequestSupplement supplement,
            IRepository<InvestmentSetting, int> settingRepository,
            IRepository<InvestmentBranchSetting, int> branchSettingRepository,
            IRepository<Customer, int> customerRepository,
            IAppSettingManager appSettingManager) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _settingRepository = settingRepository;
            _branchSettingRepository = branchSettingRepository;
            _customerRepository = customerRepository;
            _appSettingManager = appSettingManager;
        }

        public async Task<InvestmentCustomerSettingDto> Handle(GetInvestmentCustomerSettingByCode request, CancellationToken cancellationToken)
        {
            var entity = await _settingRepository.GetAll().FirstOrDefaultAsync();
            var entityDto = Mapper.Map<InvestmentCustomerSettingDto>(entity);

            var customer = await _customerRepository.GetAll().FirstOrDefaultAsync(p => p.Code == request.CustomerCode);
            if (customer == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.UserIsNotCustomer").Build();
            }
            var branchSetting = await _branchSettingRepository.GetAll().FirstOrDefaultAsync(p => p.BranchId == customer.BranchId);
            entityDto.IsPointEditable = branchSetting != null ? branchSetting.IsEditablePoint : false;
            entityDto.EnableCreateTicketFromShop = await _appSettingManager.IsEnableAsync("ENABLE_CREATE_TICKET_FROM_SHOP");
            return entityDto;
        }
    }
}