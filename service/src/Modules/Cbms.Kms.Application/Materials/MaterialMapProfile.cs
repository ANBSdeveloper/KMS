using AutoMapper;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Domain.Materials.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Materials
{
    public class MaterialMapProfile : Profile
    {
        public MaterialMapProfile()
    {
        CreateMap<UpsertMaterialDto, UpsertMaterialAction>();
    }
}
}
