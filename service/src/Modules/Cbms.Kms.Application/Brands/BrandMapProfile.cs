using AutoMapper;
using Cbms.Kms.Application.Brands.Dto;
using Cbms.Kms.Domain.Brands.Actions;

namespace Cbms.Kms.Application.Brands
{
    public class BrandMapProfile : Profile
    {
        public BrandMapProfile()
        {
            CreateMap<UpsertBrandDto, BrandUpsertAction>();
        }
    }
}