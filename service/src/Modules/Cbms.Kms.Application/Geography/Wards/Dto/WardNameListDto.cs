using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Geography.Wards;

namespace Cbms.Kms.Application.Geography.Wards.Dto
{
    public class WardNameListDto : WardDto
    {
        public string District { get; set; }
        public string Province { get; set; }
    }
}
