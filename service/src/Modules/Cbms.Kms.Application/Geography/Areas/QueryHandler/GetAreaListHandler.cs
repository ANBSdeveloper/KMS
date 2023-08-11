using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Kms.Application.Geography.Areas.Query;
using Cbms.Kms.Domain.Areas;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.Geography.Areas.QueryHandler
{
    public class GetAreaListHandler : EntityPagedQueryHandler<GetAreaList, int, Area, AreaDto>
    {
        public GetAreaListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Area> Filter(IQueryable<Area> query, GetAreaList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}