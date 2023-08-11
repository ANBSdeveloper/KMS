using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Geography.Areas.Query
{
    public class GetArea : EntityQuery<AreaDto>
    {
        public GetArea(int id) : base(id)
        {
        }
    }
}