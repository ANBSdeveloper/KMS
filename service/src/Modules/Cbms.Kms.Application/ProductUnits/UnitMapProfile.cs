using AutoMapper;
using Cbms.Kms.Application.ProductUnits.Dto;
using Cbms.Kms.Domain.ProductUnits.Actions;

namespace Cbms.Kms.Application.Units
{
    public class UnitMapProfile : Profile
    {
        public UnitMapProfile()
        {
            CreateMap<UpsertProductUnitDto, UpsertProductUnitAction>();
        }
    }
}