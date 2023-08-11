using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.RewardPackages.Commands
{
    public class RewardPackageUpsertCommand : UpsertEntityCommand<RewardPackageUpsertDto, RewardPackageDto>
    {
        public RewardPackageUpsertCommand(RewardPackageUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}