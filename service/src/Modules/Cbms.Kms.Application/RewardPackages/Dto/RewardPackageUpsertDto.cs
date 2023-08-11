using Cbms.Dto;
using Cbms.Kms.Domain.RewardPackages;
using System;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    public class RewardPackageUpsertDto : EntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public CrudListDto<RewardItemUpsertDto> RewardItemChanges { get; set; }
        public CrudListDto<RewardBranchUpSertDto> RewardBranchChanges { get; set; }

        public RewardPackageUpsertDto()
        {
            RewardItemChanges = new CrudListDto<RewardItemUpsertDto>();
            RewardBranchChanges = new CrudListDto<RewardBranchUpSertDto>();
        }
    }
}