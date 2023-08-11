using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Products.Query
{
    public class GetProductListByClass : EntityPagingResultQuery<ProductListItemDto>
    {
        public bool? IsActive { get; set; }
        public int ClassId { get; set; }
    }
}