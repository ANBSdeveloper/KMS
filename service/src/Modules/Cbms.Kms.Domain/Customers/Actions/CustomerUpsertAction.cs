using Cbms.Domain.Entities;
using System;

namespace Cbms.Kms.Domain.Customers.Actions
{
    public class CustomerUpsertAction : IEntityAction
    {
        public CustomerUpsertAction(
            bool isNew,
            string code,
            string name,
            int? branchId,
            string contactName,
            string mobilePhone,
            string phone,
            string email,
            string channelCode,
            string channelName,
            string houseNumber,
            string street,
            string address,
            int? wardId,
            int? districtId,
            int? provinceId,
            bool isActive,
            float lat,
            float lng,
            DateTime? birthday,
            DateTime? updateDate,
            int? areaId,
            int? zoneId,
            int? rsmStaffId,
            int? asmStaffId,
            int? salesSupervisorStaffId)
        {
            IsNew = isNew;
            Code = code;
            Name = name;
            BranchId = branchId;
            ContactName = contactName;
            MobilePhone = mobilePhone ?? "";
            Phone = phone;
            Email = email;
            ChannelCode = channelCode;
            ChannelName = channelName;
            HouseNumber = houseNumber;
            Street = street;
            Address = address;
            WardId = wardId;
            DistrictId = districtId;
            ProvinceId = provinceId;
            IsActive = isActive;
            Lat = lat;
            Lng = lng;
            Birthday = birthday;
            UpdateDate = updateDate;
            AreaId = areaId;
            ZoneId = zoneId;
            RsmStaffId = rsmStaffId;
            AsmStaffId = asmStaffId;
            SalesSupervisorStaffId = salesSupervisorStaffId;
        }
        public bool IsNew { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int? BranchId { get; private set; }
        public string ContactName { get; private set; }
        public string MobilePhone { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string ChannelCode { get; private set; }
        public string ChannelName { get; private set; }
        public string HouseNumber { get; private set; }
        public string Street { get; private set; }
        public string Address { get; private set; }
        public int? WardId { get; private set; }
        public int? DistrictId { get; private set; }
        public int? ProvinceId { get; private set; }
        public bool IsActive { get; private set; }
        public float Lat { get; private set; }
        public float Lng { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public int? AreaId { get; private set; }
        public int? ZoneId { get; private set; }
         public int? RsmStaffId { get; private set; }
        public int? AsmStaffId { get; private set; }
        public int? SalesSupervisorStaffId { get; private set; }

    }
}