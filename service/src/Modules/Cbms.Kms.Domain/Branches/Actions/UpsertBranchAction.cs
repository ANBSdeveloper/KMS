using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Branches.Actions
{
    public class UpsertBranchAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public int SalesOrgId { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public int? AreaId { get; private set; }
        public int? ZoneId { get; private set; }
        public int? ChannelId { get; private set; }

        public UpsertBranchAction(string code, string name, bool isActive, int salesOrgId, DateTime? updateDate, int? areaId, int?zoneId, int? channelId)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
            SalesOrgId = salesOrgId;
            UpdateDate = updateDate;
            AreaId = areaId;
            ZoneId = zoneId;
            ChannelId = channelId;
        }
    }
}