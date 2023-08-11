using Cbms.Kms.Application.Brands.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Brands.Query
{
    public class GetBrand : EntityQuery<BrandDto>
    {
        public GetBrand(int id) : base(id)
        {
        }
    }
}