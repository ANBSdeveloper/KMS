using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Application.AppSettings.Query;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cbms.Kms.Application.AppSettings.QueryHandler
{
    public class GetAppSettingListHandler : EntityPagedQueryHandler<GetAppSettingList, int, AppSetting, AppSettingDto>
    {
        public GetAppSettingListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<AppSetting> Filter(IQueryable<AppSetting> query, GetAppSettingList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Description.Contains(keyword) || x.Value.Contains(keyword));
        }
    }
}
