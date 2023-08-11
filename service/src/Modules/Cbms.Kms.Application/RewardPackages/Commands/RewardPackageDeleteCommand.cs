using Cbms.Mediator;

namespace Cbms.Kms.Application.RewardPackages.Commands
{
    public class RewardPackageDeleteCommand : DeleteEntityCommand
    {
        public RewardPackageDeleteCommand(int id) : base(id)
        {
        }
    }
}