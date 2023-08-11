using Cbms.Kms.Application.Cycles.Commands;
using Cbms.Kms.Domain.Cycles;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Cycles.CommandHandlers
{
    public class DeleteCycleCommandHandler : DeleteEntityCommandHandler<DeleteCycleCommand, Cycle>
    {
        public DeleteCycleCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}