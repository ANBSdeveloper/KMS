using Cbms.Dto;

namespace Cbms.Kms.Application.Orders.Dto
{
    public class OrderTicketDto : AuditedEntityDto
    {
        public string TicketCode { get; set; }
        public int TicketId { get; set; }
    }
}