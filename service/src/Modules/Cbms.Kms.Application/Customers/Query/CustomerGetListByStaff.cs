using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetListByStaff : EntityPagingResultQuery<CustomerByStaffListDto>
    {
        public bool? IsActive { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? RsmStaffId { get; set; }
        public int? AsmStaffId { get; set; }
        public int? SalesSupervisorStaffId { get; set; }
        public bool? HasTicketInvestment { get; set; }
        public List<int> KeyShopStatus { get; set; }
        public CustomerGetListByStaff()
        {
            KeyShopStatus = new List<int>();
        }
    }
}