using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;
namespace Cbms.Kms.Application.Products.Commands
{
    public class UpsertProductCommand : UpsertEntityCommand<UpsertProductDto, ProductBaseDto>
    {
        public UpsertProductCommand(UpsertProductDto data, string handleType) : base(data, handleType)
        {
        }
    }
}
