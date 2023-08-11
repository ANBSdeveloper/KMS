using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.ProductPoints;
using System;

namespace Cbms.Kms.Application.ProductPoints.Dto
{
    [AutoMap(typeof(ProductPoint))]
    public class ProductPointDto : AuditedEntityDto
    {
        public int ProductId { get;  set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? ProductClassId { get; set; }
        public string ProductClassName { get; set; }
        public string SubProductClassName { get; set; }
        public decimal Points { get;  set; }
        public DateTime FromDate { get;  set; }
        public DateTime ToDate { get;  set; }
        public bool IsActive { get;  set; }
    }
}