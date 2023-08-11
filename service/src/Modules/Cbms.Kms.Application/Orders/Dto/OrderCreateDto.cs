using System.Collections.Generic;

namespace Cbms.Kms.Application.Orders.Dto
{
    public class OrderCreateDto 
    {
        public string ConsumerPhone { get; set; }
        public string ConsumerName { get; set; }
        public List<OrderDetailCreateDto> OrderDetails { get; set; }
    }

    public class OrderDetailCreateDto
    {
        public string ProductCode { get; set; }
        public string QrCode { get; set; }
        public string SpoonCode { get; set; }
    }
}