using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductClasses.Commands
{
    public class UpsertProductClassCommand : UpsertEntityCommand<UpsertProductClassDto, ProductClassDto>
    {
        public UpsertProductClassCommand(UpsertProductClassDto data, string handleType) : base(data, handleType)
        {
        }
    }
}