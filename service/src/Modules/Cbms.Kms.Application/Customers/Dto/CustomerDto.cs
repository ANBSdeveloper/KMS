using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Customers;
using System;

namespace Cbms.Kms.Application.Customers.Dto
{
    [AutoMap(typeof(Customer))]
    public class CustomerDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? BranchId { get; set; }
        public string ContactName { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public int? WardId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public bool IsActive { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public DateTime? Birthday { get; set; }
        public int? UserId { get; set; }
        public bool IsKeyShop { get; set; }
        public bool IsAllowKeyShopRegister { get; set; }
        public int KeyShopStatus { get; set; }
        public string KeyShopAuthCode { get; set; }
        public int? SalesSupervisorStaffId { get; set; }
        public int? AreaId { get; set; }
        public int? ZoneId { get; set; }
        public decimal? Efficient { get; set; }
        public int? RsmStaffId { get; set; }
        public decimal? AsmStaffId { get; set; }
    }
}