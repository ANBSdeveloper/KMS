using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Kms.Application.PosmClasses.Query;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.PosmClasses.QueryHandler
{
    public class PosmClassGetListHandler : EntityPagedQueryHandler<PosmClassGetList, int, PosmClass, PosmClassDto>
    {
        public PosmClassGetListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<PosmClass> Filter(IQueryable<PosmClass> query, PosmClassGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive);
        }
    }
}