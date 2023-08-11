using AutoMapper;
using Cbms.Kms.Domain.Products;

namespace Cbms.Kms.Application.Products.Dto
{
    [AutoMap(typeof(ProductInfo))]
    public class ProductInfoDto
    {
        public string QrCode { get; private set; }
        public string ProductCode { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public int Quantity { get; private set; }
        public decimal Point { get; private set; }
    }
}
