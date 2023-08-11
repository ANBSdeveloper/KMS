namespace Cbms.Kms.Application.Customers.Dto
{
    public class CustomerByStaffListDto : CustomerDto
    {
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string SalesSupervisorStaffCode { get; set; }
        public string SalesSupervisorStaffName { get; set; }

    }
}