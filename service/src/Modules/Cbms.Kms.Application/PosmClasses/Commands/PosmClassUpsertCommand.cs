using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmClasses.Commands
{
    public class PosmClassUpsertCommand : UpsertEntityCommand<PosmClassUpsertDto, PosmClassDto>
    {
        public PosmClassUpsertCommand(PosmClassUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}