using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Products.Query
{
    public class GetProductList : EntityPagingResultQuery<ProductListItemDto>
    {
        public bool? IsActive { get; set; }
        public string ProductClassCode { get; set; }
    }
}