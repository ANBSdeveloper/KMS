using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Cycles.Commands
{
    public class UpsertCycleCommand : UpsertEntityCommand<UpsertCycleDto, CycleDto>
    {
        public UpsertCycleCommand(UpsertCycleDto data, string handleType) : base(data, handleType)
        {
        }
    }
}