using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Customers.Dto
{
    public class CustomerApproveKeyShopListDto
    {
        public string Id { get; set; }
        public bool IsSelected { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public int KeyShopStatus { get; set; }
        public int KeyShopRegisterStaffId { get; set; }
        public string StaffName { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string Email { get; set; }
        public string KeyShopAuthCode { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
    }
}
