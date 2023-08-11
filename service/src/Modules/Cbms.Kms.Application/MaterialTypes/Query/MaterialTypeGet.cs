using Cbms.Kms.Application.MaterialTypes.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.MaterialTypes.Query
{
    public class MaterialTypeGet : EntityQuery<MaterialTypeDto>
    {
        public MaterialTypeGet(int id) : base(id)
        {
        }
    }
}
