using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Orders.Actions
{
    public class OrderTicketCreateAction : IEntityAction
    {
        public OrderTicketCreateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int ticketId,
            string ticketCode, 
            string qrCode)
        {
            TicketId = ticketId;
            TicketCode = ticketCode;
            QrCode = qrCode;
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
        }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int TicketId { get; private set; }
        public string TicketCode { get; private set; }
        public string QrCode { get; private set; }
    }
}