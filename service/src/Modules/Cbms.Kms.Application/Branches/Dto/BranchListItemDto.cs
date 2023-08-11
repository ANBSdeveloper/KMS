using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Branches;

namespace Cbms.Kms.Application.Branches.Dto
{
    [AutoMap(typeof(Branch))]
    public class BranchListItemDto : BranchDto
    {
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
        public string ChannelName { get; set; }
        public int? ProvinccId { get; set; }
        public string ProvinceName { get; set; }
    }
}