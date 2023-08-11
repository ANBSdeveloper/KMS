using Cbms.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.RewardPackages.Actions
{
    public class RewardPackageUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public RewardPackageType Type { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public List<RewardItemUpsertAction> UpsertItems { get; set; }
        public List<int> DeleteItems { get; set; }
        public List<RewardBranchUpsertAction> UpsertRewardBranches { get; set; }
        public List<int> DeleteRewardBranches { get; set; }

        public RewardPackageUpsertAction(
            string code,
            string name,
            bool isActive,
            RewardPackageType type,
            DateTime fromDate,
            DateTime toDate,
            List<RewardItemUpsertAction> upsertItems,
            List<int> deleteItems,
            List<RewardBranchUpsertAction> upsertRewardBranches,
            List<int> deleteRewardBranches)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
            Name = name;
            Type = type;
            FromDate = fromDate;
            ToDate = toDate;
            UpsertItems = upsertItems;
            DeleteItems = deleteItems;
            UpsertRewardBranches = upsertRewardBranches;
            DeleteRewardBranches = deleteRewardBranches;
        }
    }
}