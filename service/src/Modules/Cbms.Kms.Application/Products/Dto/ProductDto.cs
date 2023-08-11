using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Cbms.Kms.Domain.Products;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Products.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductDto : ProductBaseDto
    {
    }
}