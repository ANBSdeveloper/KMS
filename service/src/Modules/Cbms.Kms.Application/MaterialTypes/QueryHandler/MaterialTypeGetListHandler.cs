using Cbms.Kms.Application.MaterialTypes.Dto;
using Cbms.Kms.Application.MaterialTypes.Query;
using Cbms.Kms.Domain.MaterialTypes;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.MaterialTypes.QueryHandler
{
    public class MaterialTypeGetListHandler : EntityPagedQueryHandler<MaterialTypeGetList, int, MaterialType, MaterialTypeDto>
    {
        public MaterialTypeGetListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<MaterialType> Filter(IQueryable<MaterialType> query, MaterialTypeGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
    }
}