using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Staffs.Query
{
    public class StaffGetAsmList : EntityPagingResultQuery<StaffListDto>
    {
        public int? SupervisorId { get; set; }
    }
}