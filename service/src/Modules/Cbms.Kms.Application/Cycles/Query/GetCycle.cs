using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Cycles.Query
{
    public class GetCycle : EntityQuery<CycleDto>
    {
        public GetCycle(int id) : base(id)
        {
        }
    }
}