using AutoMapper;
using Cbms.Kms.Domain.Materials;

namespace Cbms.Kms.Application.Materials.Dto
{
    public class MaterialListItemDto : MaterialDto
    {
        public string MaterialTypeName { get; set; }
    }
}
