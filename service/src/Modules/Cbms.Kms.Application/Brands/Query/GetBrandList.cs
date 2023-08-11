using Cbms.Kms.Application.Brands.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Brands.Query
{
    public class GetBrandList : EntityPagingResultQuery<BrandDto>
    {
        public bool? IsActive { get; set; }
    }
}