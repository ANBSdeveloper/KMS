using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Staffs.Actions
{
    public class StaffUpsertAction : IEntityAction
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int SalesOrgId { get; private set; }
        public string StaffTypeCode { get; private set; }
        public string StaffTypeName { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public string MobilePhone { get; private set; }
        public DateTime? Birthday { get; private set; }
        public string Email { get; private set; }
        public int? UserId { get; private set; }
        public int? AreaId { get; private set; }
        public int? ZoneId { get; private set; }
        public bool IsActive { get; private set; }

        public StaffUpsertAction(string code, string name, DateTime updateDate, int salesOrgId, string staffTypeCode, string staffTypeName, string mobilePhone, DateTime? birthday, string email, int? userId, int? areaId, int? zoneId, bool isActive)
        {
            Code = code;
            Name = name;
            SalesOrgId = salesOrgId;
            StaffTypeCode = staffTypeCode;
            StaffTypeName = staffTypeName;
            UpdateDate = updateDate;
            MobilePhone = mobilePhone;
            Birthday = birthday;
            Email = email;
            UserId = userId;
            AreaId = areaId;
            ZoneId = zoneId;
            IsActive = isActive;
        }
    }
}
