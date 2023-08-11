using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Orders.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Orders
{
    public class OrderTicket : AuditedEntity
    {
        public int TicketId { get; private set; }
        public string TicketCode { get; private set; }
        public int OrderId { get; private set; }
        public string QrCode { get; private set; }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case OrderTicketCreateAction upsertAction:
                    await CreateAsync(upsertAction);
                    break;
            }
        }

        public async Task CreateAsync(OrderTicketCreateAction action)
        {
            TicketId = action.TicketId;
            TicketCode = action.TicketCode;
            QrCode = action.QrCode;
        }
    }
}