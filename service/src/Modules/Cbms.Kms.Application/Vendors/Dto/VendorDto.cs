using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Vendors;

namespace Cbms.Kms.Application.Vendors.Dto
{
    [AutoMap(typeof(Vendor))]
    public class VendorDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public int? ZoneId { get; set; }
        public string TaxReg { get; set; }
        public string Representative { get; set; }
    }
}
