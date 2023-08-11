using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmClasses.Query
{
    public class PosmClassGetList : EntityPagingResultQuery<PosmClassDto>
    {
        public bool? IsActive { get; set; }
    }
}
