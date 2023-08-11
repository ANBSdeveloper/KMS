using AutoMapper;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmPrices.Dto
{
    [AutoMap(typeof(PosmPriceHeader))]
    public class PosmPriceHeaderListDto : PosmPriceHeaderBaseDto
    {

    }
}