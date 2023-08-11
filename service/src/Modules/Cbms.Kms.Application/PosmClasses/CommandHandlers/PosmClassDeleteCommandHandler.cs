using Cbms.Kms.Application.PosmClasses.Commands;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmClasses.CommandHandlers
{
    public class PosmClassDeleteCommandHandler : DeleteEntityCommandHandler<PosmClassDeleteCommand, PosmClass>
    {
        public PosmClassDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}