using AutoMapper;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Mediator;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    [AutoMap(typeof(RewardPackage))]
    public class RewardPackageListDto : RewardPackageBaseDto
    {

    }
}