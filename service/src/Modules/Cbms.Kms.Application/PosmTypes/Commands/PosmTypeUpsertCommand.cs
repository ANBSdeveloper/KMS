using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmTypes.Commands
{
    public class PosmTypeUpsertCommand : UpsertEntityCommand<PosmTypeUpsertDto, PosmTypeDto>
    {
        public PosmTypeUpsertCommand(PosmTypeUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}