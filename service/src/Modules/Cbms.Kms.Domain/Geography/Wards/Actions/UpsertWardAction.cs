using Cbms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Domain.Geography.Wards.Actions
{
    public class UpsertWardAction : IEntityAction
    {
        public string Code { get; private set; }

        public string Name { get; private set; }
        public int DistrictId { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public UpsertWardAction(string code, string name, int districtId, DateTime updateDate)
        {
            Code = code;
            Name = name;
            DistrictId = districtId;
            UpdateDate = updateDate;
        }
    }
}
