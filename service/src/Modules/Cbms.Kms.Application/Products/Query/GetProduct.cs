using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Products.Query
{
    public class GetProduct : EntityQuery<ProductBaseDto>
    {
        public GetProduct(int id) : base(id)
        {
        }
    }
}