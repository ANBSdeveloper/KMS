using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Kms.Application.PosmTypes.Query;
using Cbms.Kms.Domain.PosmTypes;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.PosmTypes.QueryHandler
{
    public class PosmTypeGetListHandler : EntityPagedQueryHandler<PosmTypeGetList, int, PosmType, PosmTypeDto>
    {
        public PosmTypeGetListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<PosmType> Filter(IQueryable<PosmType> query, PosmTypeGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword))
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive);
        }
    }
}