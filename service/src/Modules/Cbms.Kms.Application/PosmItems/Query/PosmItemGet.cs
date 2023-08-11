using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmItems.Query
{
    public class PosmItemGet : EntityQuery<PosmItemDto>
    {
        public PosmItemGet(int id) : base(id)
        {
        }
    }
}
