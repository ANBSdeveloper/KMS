using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Vendors;

namespace Cbms.Kms.Application.Vendors.Dto
{
    [AutoMap(typeof(Vendor))]
    public class VendorListDto : VendorDto
    {
        public string Zone { get; set; }    
    }
}
