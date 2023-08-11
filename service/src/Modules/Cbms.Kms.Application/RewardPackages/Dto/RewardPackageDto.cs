using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Kms.Domain.RewardPackages;
using System.Collections.Generic;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    [AutoMap(typeof(RewardPackage))]
    public class RewardPackageDto : RewardPackageBaseDto
    {
        [Ignore]
        public List<RewardItemDto> RewardItems { get; set; }

        [Ignore]
        public List<RewardBranchDto> RewardBranches { get; set; }
    }
}