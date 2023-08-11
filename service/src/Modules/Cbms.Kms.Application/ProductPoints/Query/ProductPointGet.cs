using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductPoints.Query
{
    public class ProductPointGet : EntityQuery<ProductPointDto>
    {
        public ProductPointGet(int id) : base(id)
        {
        }
    }
}