using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductUnits.Commands
{
    public class UpsertProductUnitCommand : UpsertEntityCommand<UpsertProductUnitDto, ProductUnitDto>
    {
        public UpsertProductUnitCommand(UpsertProductUnitDto data, string handleType) : base(data, handleType)
        {
        }
    }
}