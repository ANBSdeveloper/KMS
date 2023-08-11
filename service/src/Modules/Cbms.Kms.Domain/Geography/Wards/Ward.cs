using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Geography.Wards.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Geography.Wards
{
    public class Ward : AuditedAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int DistrictId { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public Ward()
        {
        }

        public static Ward Create()
        {
            return new Ward();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case UpsertWardAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(UpsertWardAction action)
        {
            Code = action.Code;
            Name = action.Name;
            DistrictId = action.DistrictId;
            UpdateDate = action.UpdateDate;
        }
    }
}
