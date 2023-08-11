using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Mediator;
namespace Cbms.Kms.Application.Geography.Wards.Query
{
    public class GetWard : EntityQuery<WardDto>
    {
        public GetWard(int id) : base(id)
        {
        }
    }
}
