using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Branches;

namespace Cbms.Kms.Application.Branches.Dto
{
    [AutoMap(typeof(Branch))]
    public class BranchDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public int SalesOrgId { get; set; }
        public int ZoneId { get; set; }
        public bool IsActive { get; set; }
        public int? ChannelId { get; set; }
    }
}