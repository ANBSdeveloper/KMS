using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Kms.Application.Cycles.Query;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Cycles;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System;
using System.Linq;

namespace Cbms.Kms.Application.Cycles.QueryHandlers
{
    public class GetCycleListHandler : EntityPagedQueryHandler<GetCycleList, int, Cycle, CycleDto>
    {
        private readonly IAppSettingManager _appSettingManager;
        public GetCycleListHandler(IRequestSupplement supplement, IAppSettingManager appSettingManager) : base(supplement)
        {
            _appSettingManager = appSettingManager;
        }

        protected override IQueryable<Cycle> Filter(IQueryable<Cycle> query, GetCycleList request)
        {
            var keyword = request.Keyword;
            var filter =  query.WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                    .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Number.Contains(keyword) || x.Number.Contains(keyword));

            int cycleMonths = 0;
            var cycleMonthsConfig = _appSettingManager.GetAsync("CYCLE_MONTHS").GetAwaiter().GetResult();
            if (int.TryParse(cycleMonthsConfig, out cycleMonths))
            {
                var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                fromDate = fromDate.AddMonths(-1 * cycleMonths);
                filter = filter.Where(p => p.FromDate >= fromDate);
            }

            return filter;

        }
    }
}