using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmItems.CommandHandlers
{
    public class PosmItemDeleteCommandHandler : DeleteEntityCommandHandler<PosmItemDeleteCommand, PosmItem>
    {
        public PosmItemDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}