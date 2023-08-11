using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Kms.Domain.PosmPrices;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmPrices.Dto
{
    [AutoMap(typeof(PosmPriceHeader))]
    public class PosmPriceHeaderDto : PosmPriceHeaderBaseDto
    {
        [Ignore]
        public List<PosmPriceDetailDto> Details { get; set; }
    }
}