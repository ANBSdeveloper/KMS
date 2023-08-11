using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Staffs.Query
{
    public class StaffGetListByRole : EntityPagingResultQuery<StaffListDto>
    {
        public int? SupervisorId { get; set; }
        public string StaffTypeCode { get; set; }
    }
}