using Cbms.Dto;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    public class RewardBranchDto : AuditedEntityDto
    {
        public bool IsSelected { get; set; }
        public int BranchId { get; set; }
        public string AreaName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string ZoneName { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
    }
}