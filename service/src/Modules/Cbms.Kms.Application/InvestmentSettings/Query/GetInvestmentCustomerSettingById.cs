using Cbms.Kms.Application.InvestmentSettings.Dto;
using MediatR;

namespace Cbms.Kms.Application.InvestmentSettings.Query
{
    public class GetInvestmentCustomerSettingById : IRequest<InvestmentCustomerSettingDto>
    {
        public int CustomerId { get; set; }
    }
}
