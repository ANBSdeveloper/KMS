using AutoMapper;
using Cbms.Kms.Domain.Products;

namespace Cbms.Kms.Application.Products.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductItemDto : ProductBaseDto
    {
        public string BrandName { get; set; }
        public string ProductClassName { get; set; }
        public string SubProductClassName { get; set; }
        public string Status { get; set; }
    }
}