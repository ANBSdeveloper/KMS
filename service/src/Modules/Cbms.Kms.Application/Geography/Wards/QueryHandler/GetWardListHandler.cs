using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Kms.Application.Geography.Wards.Query;
using Cbms.Kms.Domain.Geography.Wards;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.Geography.Wards.QueryHandler
{
    public class GetWardListHandler : EntityPagedQueryHandler<GetWardList, int, Ward, WardDto>
    {
        public GetWardListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Ward> Filter(IQueryable<Ward> query, GetWardList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                .WhereIf(request.DistrictId.HasValue, x => x.DistrictId == (int)request.DistrictId);

        }
    }
}
