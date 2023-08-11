using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.ProductClasses;

namespace Cbms.Kms.Application.ProductClasses.Dto
{
    [AutoMap(typeof(ProductClass))]
    public class ProductClassDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string RewardCode { get; set; }

        public bool IsActive { get; set; }
    }
}