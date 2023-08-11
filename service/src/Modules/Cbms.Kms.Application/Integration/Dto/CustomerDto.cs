using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Integration.Dto
{
    public class CustomerListDto
    {
        [JsonProperty("List_Shop")]
        public List<CustomerDto> Items { get; set; }
    }

    public class CustomerDto
    {
        public int SalesOrgId { get; set; }

        [JsonProperty("cust_code")]
        public string Code { get; set; }

        [JsonProperty("cust_name")]
        public string Name { get; set; }

        [JsonProperty("contact_Name")]
        public string ContactName { get; set; }

        [JsonProperty("mobi_phone")]
        public string MobiPhone { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("channel_type_code")]
        public string ChannelTypeCode { get; set; }

        [JsonProperty("channel_type_name")]
        public string ChannelTypeName { get; set; }

        [JsonProperty("house_number")]
        public string HouseNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("dia_chi")]
        public string Address { get; set; }

        [JsonProperty("ward")]
        public string WardCode { get; set; }

        [JsonProperty("district_code")]
        public string DistrictCode { get; set; }

        [JsonProperty("province_code")]
        public string ProvinceCode { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("lat")]
        public float Lat { get; set; }

        [JsonProperty("lng")]
        public float Lng { get; set; }

        [JsonProperty("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonProperty("updateDate")]
        public DateTime? UpdateDate { get; set; }
        
    }
}