using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmTypes.Query
{
    public class PosmTypeGetList : EntityPagingResultQuery<PosmTypeDto>
    {
        public bool? IsActive { get; set; }
    }
}
