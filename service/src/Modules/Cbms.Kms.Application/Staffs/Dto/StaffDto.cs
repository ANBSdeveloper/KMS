using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Staffs;
using System;

namespace Cbms.Kms.Application.Staffs.Dto
{
    [AutoMap(typeof(Staff))]
    public class StaffDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int SalesOrgId { get; set; }
        public string StaffTypeCode { get; set; }
        public string StaffTypeName { get; set; }
        public DateTime UpdateDate { get; set; }
        public string MobilePhone { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public int? AreaId { get; set; }
        public int? ZoneId { get; set; }
    }
}