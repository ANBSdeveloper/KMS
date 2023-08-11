using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemGetListByUser : EntityPagingResultQuery<PosmInvestmentItemExtDto>
    {
        public List<int> Status { get; set; }
        public int? CycleId { get; set; }
        public int? StaffId { get; set; }
        public int? WardId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }

        public PosmInvestmentItemGetListByUser()
        {
            Status = new List<int>();
        }
    }
}