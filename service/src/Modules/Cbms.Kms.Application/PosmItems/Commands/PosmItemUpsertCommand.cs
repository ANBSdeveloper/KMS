using Cbms.Dto;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Mediator;
using static Cbms.Kms.Application.PosmItems.Commands.PosmItemUpsertCommand;

namespace Cbms.Kms.Application.PosmItems.Commands
{
  
    public class PosmItemUpsertCommand : UpsertEntityCommand<PosmItemUpsertDto, PosmItemDto>
    {
        public class PosmItemUpsertDto : PosmItemDto
        {
            public CrudListDto<PosmCatalogDto> CatalogChanges { get; set; }

            public PosmItemUpsertDto()
            {
                CatalogChanges = new CrudListDto<PosmCatalogDto>();
            }
        }
        public PosmItemUpsertCommand(PosmItemUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}