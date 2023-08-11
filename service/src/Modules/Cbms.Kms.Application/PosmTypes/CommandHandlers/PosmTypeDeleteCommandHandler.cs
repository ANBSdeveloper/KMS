using Cbms.Kms.Application.PosmTypes.Commands;
using Cbms.Kms.Domain.PosmTypes;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmTypes.CommandHandlers
{
    public class PosmTypeDeleteCommandHandler : DeleteEntityCommandHandler<PosmTypeDeleteCommand, PosmType>
    {
        public PosmTypeDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}