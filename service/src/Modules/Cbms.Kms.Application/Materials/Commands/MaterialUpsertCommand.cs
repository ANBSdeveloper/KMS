using Cbms.Kms.Application.Materials.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Materials.Commands
{
    public class MaterialUpsertCommand : UpsertEntityCommand<UpsertMaterialDto, MaterialDto>
    {
        public MaterialUpsertCommand(UpsertMaterialDto data, string handleType) : base(data, handleType)
        {
        }
    }
}
