using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Mediator;
using System;

namespace Cbms.Kms.Application.PosmPrices.Query
{
    public class PosmPriceHeaderGetList : EntityPagingResultQuery<PosmPriceHeaderListDto>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
    }
}