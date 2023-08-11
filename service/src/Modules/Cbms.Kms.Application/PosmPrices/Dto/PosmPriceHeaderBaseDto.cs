using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmPrices;
using System;

namespace Cbms.Kms.Application.PosmPrices.Dto
{
    [AutoMap(typeof(PosmPriceHeader))]
    public class PosmPriceHeaderBaseDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}