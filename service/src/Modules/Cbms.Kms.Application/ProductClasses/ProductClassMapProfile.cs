using AutoMapper;
using Cbms.Kms.Application.ProductClasses.Dto;
using Cbms.Kms.Domain.ProductClasses.Actions;

namespace Cbms.Kms.Application.ProductClasses
{
    public class ProductClassMapProfile : Profile
    {
        public ProductClassMapProfile()
        {
            CreateMap<UpsertProductClassDto, ProductClassUpsertAction>();
        }
    }
}