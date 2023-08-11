using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmItems.Query
{
    public class PosmCatalogGetList : EntityPagingResultQuery<PosmCatalogListDto>
    {
        public int PosmItemId { get; set; }
    }
}
