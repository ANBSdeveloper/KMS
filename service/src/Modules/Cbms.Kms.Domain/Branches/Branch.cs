using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Branches.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Branches
{
    public class Branch : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public int SalesOrgId { get; private set; }
        public int? AreaId { get; set; }
        public int? ZoneId { get; set; }
        public int? ChannelId { get; set; }
        public Branch()
        {
        }

        public static Branch Create()
        {
            return new Branch();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertBranchAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertBranchAction action)
        {
            Code = action.Code;
            Name = action.Name;
            IsActive = action.IsActive;
            SalesOrgId = action.SalesOrgId;
            UpdateDate = action.UpdateDate;
            AreaId = action.AreaId;
            ZoneId = action.ZoneId;
            ChannelId = action.ChannelId;
        }
    }
}