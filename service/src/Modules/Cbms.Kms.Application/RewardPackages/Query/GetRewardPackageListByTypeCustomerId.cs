using Cbms.Kms.Application.RewardPackages.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.RewardPackages.Query
{
    public class GetRewardPackageListByTypeCustomerId : EntityPagingResultQuery<RewardPackageListDto>
    {
        public int Type { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ValidDate { get; set; }
    }
}
