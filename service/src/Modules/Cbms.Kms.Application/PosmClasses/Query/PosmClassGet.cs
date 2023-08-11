using Cbms.Kms.Application.PosmClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmClasses.Query
{
    public class PosmClassGet : EntityQuery<PosmClassDto>
    {
        public PosmClassGet(int id) : base(id)
        {
        }
    }
}
