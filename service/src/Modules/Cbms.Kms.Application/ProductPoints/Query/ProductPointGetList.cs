using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductPoints.Query
{
    public class ProductPointGetList : EntityPagingResultQuery<ProductPointListItemDto>
    {
        public int? ProductClassId { get; set; }
        public bool? IsActive { get; set; }
    }
}