using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.RewardPackages.Query
{
    public class GetRewardPackageList : EntityPagingResultQuery<RewardPackageListDto>
    {
        public bool? IsActive { get; set; }
    }
}