using Cbms.Kms.Application.InvestmentSettings.Dto;
using MediatR;

namespace Cbms.Kms.Application.InvestmentSettings.Query
{
    public class GetInvestmentSetting : IRequest<InvestmentSettingDto>
    {
        public GetInvestmentSetting()
        {
        }
    }
}
