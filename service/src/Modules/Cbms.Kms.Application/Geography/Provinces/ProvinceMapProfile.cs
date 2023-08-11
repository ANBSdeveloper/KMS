using AutoMapper;
using Cbms.Kms.Application.Geography.Provinces.Dto;
using Cbms.Kms.Domain.Geography.Provinces.Actions;

namespace Cbms.Kms.Application.Geography.Provinces
{
    public class ProvinceMapProfile : Profile
    {
        public ProvinceMapProfile()
        {
            CreateMap<UpsertProvinceDto, UpsertProvinceAction>();
        }
    }
}