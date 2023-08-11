using AutoMapper;
using Cbms.Kms.Domain.ProductPoints;

namespace Cbms.Kms.Application.ProductPoints.Dto
{
    [AutoMap(typeof(ProductPoint))]
    public class ProductPointListItemDto : ProductPointDto
    { 

    }
}