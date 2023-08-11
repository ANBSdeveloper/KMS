using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductClasses.Query
{
    public class GetProductClass : EntityQuery<ProductClassDto>
    {
        public GetProductClass(int id) : base(id)
        {
        }
    }
}