using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.RewardPackages.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.RewardPackages
{
    public class RewardPackage : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public RewardPackageType Type { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public decimal TotalTickets { get; private set; }
        public decimal TotalAmount { get; private set; }

        public IReadOnlyCollection<RewardItem> RewardItems => _rewardItems;        
        public List<RewardItem> _rewardItems = new List<RewardItem>();        

        public IReadOnlyCollection<RewardBranch> RewardBranches => _rewardBranches;
        public List<RewardBranch> _rewardBranches = new List<RewardBranch>();

        private RewardPackage()
        {
        }

        public static RewardPackage Create()
        {
            return new RewardPackage();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case RewardPackageUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(RewardPackageUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
            Type = (RewardPackageType)action.Type;
            FromDate = action.FromDate;
            ToDate = action.ToDate;
    
            foreach (var id in action.DeleteItems)
            {
                var rewardItem = _rewardItems.FirstOrDefault(p => p.Id == id);
                if (rewardItem != null)
                {
                    _rewardItems.Remove(rewardItem);
                }
            }

            foreach (var item in action.UpsertItems)
            {
                RewardItem rewardItem = null;

                if (!item.Id.IsNew())
                {
                    rewardItem = _rewardItems.FirstOrDefault(p => p.Id == item.Id);
                    if (rewardItem == null)
                    {
                        throw new EntityNotFoundException(typeof(RewardItem), item.Id);
                    }
                }
                else
                {
                    rewardItem = _rewardItems.FirstOrDefault(p => p.Code == item.Code);
                    if (rewardItem == null)
                    {
                        rewardItem = RewardItem.Create();
                        _rewardItems.Add(rewardItem);
                    }
                }

                await rewardItem.ApplyActionAsync(item);
            }

            TotalTickets = _rewardItems.Sum(p=>p.Quantity);
            TotalAmount = _rewardItems.Sum(p => p.Price * p.Quantity);

            foreach (var id in action.DeleteRewardBranches)
            {
                var detail = _rewardBranches.FirstOrDefault(p => p.Id == id);
                if (detail != null)
                {
                    _rewardBranches.Remove(detail);
                }
            }


            foreach (var item in action.UpsertRewardBranches)
            {
                RewardBranch detail = null;

                //var branch = _branches.FirstOrDefault(p => p.Id == item.Id);
                //if (branch == null)
                //{
                //    throw new EntityNotFoundException(typeof(Branch), item.BranchId);
                //}

                if (!item.Id.IsNew())
                {
                    detail = _rewardBranches.FirstOrDefault(p => p.Id == item.Id);
                    if (detail == null)
                    {
                        throw new EntityNotFoundException(typeof(RewardBranch), item.Id);
                    }
                }
                else
                {
                    detail = _rewardBranches.FirstOrDefault(p => p.BranchId == item.BranchId);
                    if (detail == null)
                    {
                        detail = RewardBranch.Create();
                        _rewardBranches.Add(detail);
                    }
                 
                }

                await detail.ApplyActionAsync(item);
            }
        }
    }
}