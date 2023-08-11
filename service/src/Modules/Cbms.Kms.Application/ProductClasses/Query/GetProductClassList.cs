using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductClasses.Query
{
    public class GetProductClassList : EntityPagingResultQuery<ProductClassDto>
    {
        public bool? IsActive { get; set; }
    }
}