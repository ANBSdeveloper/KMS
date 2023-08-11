using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Staffs.Query
{
    public class StaffGet : EntityQuery<StaffDto>
    {
        public StaffGet(int id) : base(id)
        {
        }
    } 
}
