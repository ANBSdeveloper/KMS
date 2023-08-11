using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.RewardPackages.Query
{
    public class GetRewardPackage : EntityQuery<RewardPackageDto>
    {
        public GetRewardPackage(int id) : base(id)
        {
        }
    }
}