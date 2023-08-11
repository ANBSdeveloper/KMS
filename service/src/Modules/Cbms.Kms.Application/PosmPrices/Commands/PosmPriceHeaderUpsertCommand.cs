using Cbms.Dto;
using Cbms.Kms.Application.PosmPrices.Dto;
using Cbms.Mediator;
using System;
using static Cbms.Kms.Application.PosmPrices.Commands.PosmPriceUpsertCommand;

namespace Cbms.Kms.Application.PosmPrices.Commands
{
    public class PosmPriceUpsertCommand : UpsertEntityCommand<PosmPriceUpsertDto, PosmPriceHeaderDto>
    {
        public class PosmPriceUpsertDto : EntityDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public CrudListDto<PosmPriceDetailUpsertDto> DetailChanges { get; set; }

            public PosmPriceUpsertDto()
            {
                DetailChanges = new CrudListDto<PosmPriceDetailUpsertDto>();
            }
        }

        public class PosmPriceDetailUpsertDto : AuditedEntityDto
        {
            public int PosmItemId { get; set; }
            public decimal Price { get; set; }
        }

        public PosmPriceUpsertCommand(PosmPriceUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}