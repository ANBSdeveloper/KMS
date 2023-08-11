using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmItems.Query
{
    public class PosmItemGetList : EntityPagingResultQuery<PosmItemListDto>
    {
        public bool? IsActive { get; set; }
        public int? PosmClassId { get; set; }
    }
}
