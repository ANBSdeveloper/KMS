using Cbms.Kms.Application.PosmTypes.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmTypes.Query
{
    public class PosmTypeGet : EntityQuery<PosmTypeDto>
    {
        public PosmTypeGet(int id) : base(id)
        {
        }
    }
}
