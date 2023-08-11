using System;

namespace Cbms.Kms.Application.Customers.Dto
{
    public class CustomerValidateActivationKeyShopDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string AuthCode { get; set; }
        public string MobilePhone { get; set; }
        public DateTime Birthday { get; set; }
    }
}