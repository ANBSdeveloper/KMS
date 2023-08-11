using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.ProductUnits;

namespace Cbms.Kms.Application.ProductUnits.Dto
{
    [AutoMap(typeof(ProductUnit))]
    public class ProductUnitDto : AuditedEntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

    }
}