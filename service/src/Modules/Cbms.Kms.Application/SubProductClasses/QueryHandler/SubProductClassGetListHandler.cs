using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Kms.Application.SubProductClasses.Query;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.SubProductClasses.QueryHandlers
{
    public class SubProductClassGetListHandler : EntityPagedQueryHandler<SubProductClassGetList, int, SubProductClass, SubProductClassDto>
    {
        public SubProductClassGetListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<SubProductClass> Filter(IQueryable<SubProductClass> query, SubProductClassGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive);
        }
    }
}