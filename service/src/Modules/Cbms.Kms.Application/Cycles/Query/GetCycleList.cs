using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Cycles.Query
{
    public class GetCycleList : EntityPagingResultQuery<CycleDto>
    {
        public bool? IsActive { get; set; }
        public bool? UseLimitConfig { get; set; }
    }
}