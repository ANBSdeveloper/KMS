using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Products.Query
{
    public class GetProductItem : EntityQuery<ProductItemDto>
    {
        public GetProductItem(int id) : base(id)
        {
        }
    }
}