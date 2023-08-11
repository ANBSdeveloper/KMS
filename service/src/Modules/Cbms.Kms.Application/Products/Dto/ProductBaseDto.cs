using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Products;
using System;

namespace Cbms.Kms.Application.Products.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductBaseDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Unit { get; set; }
        public string CaseUnit { get; set; }
        public int PackSize { get; set; }
        public int? ProductClassId { get; set; }
        public int? SubProductClassId { get; set; }
        public int? BrandId { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Point { get; set; }
    }
}