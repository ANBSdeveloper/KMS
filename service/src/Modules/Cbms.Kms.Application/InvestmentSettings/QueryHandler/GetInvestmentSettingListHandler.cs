using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.QueryHandler
{
    public class GetInvestmentSettingListHandler : EntityPagedQueryHandler<GetInvestmentSettingList, int, InvestmentSetting, InvestmentSettingDto>
    {
        public GetInvestmentSettingListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<InvestmentSetting> Filter(IQueryable<InvestmentSetting> query, GetInvestmentSettingList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.MaxInvestAmount.ToString().Contains(keyword) || x.AmountPerPoint.ToString().Contains(keyword));
        }
    }
}
