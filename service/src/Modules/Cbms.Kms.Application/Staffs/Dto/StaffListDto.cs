using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Staffs;
using System;

namespace Cbms.Kms.Application.Staffs.Dto
{
    [AutoMap(typeof(Staff))]
    public class StaffListDto : EntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string StaffTypeCode { get; set; }
        public string StaffTypeName { get; set; }
        public DateTime UpdateDate { get; set; }
        public string MobilePhone { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}