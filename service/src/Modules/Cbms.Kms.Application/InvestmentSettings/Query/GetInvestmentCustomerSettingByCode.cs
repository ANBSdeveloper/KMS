using Cbms.Kms.Application.InvestmentSettings.Dto;
using MediatR;

namespace Cbms.Kms.Application.InvestmentSettings.Query
{
    public class GetInvestmentCustomerSettingByCode : IRequest<InvestmentCustomerSettingDto>
    {
        public string CustomerCode { get; set; }
    }
}
