using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.RewardPackages;
using System;

namespace Cbms.Kms.Application.RewardPackages.Dto
{
    [AutoMap(typeof(RewardPackage))]
    public class RewardPackageBaseDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTickets { get; set; }
    }
}