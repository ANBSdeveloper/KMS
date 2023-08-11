using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetList : EntityPagingResultQuery<CustomerDto>
    {
        public bool? IsActive { get; set; }
        public bool? IsKeyShop { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? StaffId { get; set; }
        public bool? HasTicketInvestment { get; set; }
        public List<int> KeyShopStatus { get; set; }
        public CustomerGetList()
        {
            KeyShopStatus = new List<int>();
        }
    }
}