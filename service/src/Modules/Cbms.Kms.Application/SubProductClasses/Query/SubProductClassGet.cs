using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.SubProductClasses.Query
{
    public class SubProductClassGet : EntityQuery<SubProductClassDto>
    {
        public SubProductClassGet(int id) : base(id)
        {
        }
    }
}