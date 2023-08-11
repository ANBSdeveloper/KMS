using Cbms.Kms.Application.Products.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Products.Query
{
    public class GetProductByQrCode: QueryBase, IRequest<ProductInfoDto>
    {
        public string QrCode { get; set; }
        public bool? SmallUnitRequire { get; set; }
    }
}
