using Cbms.Kms.Application.ProductPoints.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.ProductPoints.Commands
{
    public class ProductPointUpsertCommand : UpsertEntityCommand<ProductPointUpsertDto, ProductPointDto>
    {
        public ProductPointUpsertCommand(ProductPointUpsertDto data, string handleType) : base(data, handleType)
        {
        }

        public ProductPointUpsertCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}