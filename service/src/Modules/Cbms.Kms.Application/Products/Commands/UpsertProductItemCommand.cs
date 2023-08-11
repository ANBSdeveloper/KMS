using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;
namespace Cbms.Kms.Application.Products.Commands
{
    public class UpsertProductItemCommand : UpsertEntityCommand<UpsertProductDto, ProductItemDto>
    {
        public UpsertProductItemCommand(UpsertProductDto data, string handleType) : base(data, handleType)
        {
        }
    }
}
