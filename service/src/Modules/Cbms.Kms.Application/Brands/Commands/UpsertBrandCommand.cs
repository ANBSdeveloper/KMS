using Cbms.Kms.Application.Brands.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Brands.Commands
{
    public class UpsertBrandCommand : UpsertEntityCommand<UpsertBrandDto, BrandDto>
    {
        public UpsertBrandCommand(UpsertBrandDto data, string handleType) : base(data, handleType)
        {
        }
    }
}