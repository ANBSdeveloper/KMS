using Cbms.Mediator;
using static Cbms.Kms.Domain.Orders.Actions.OrderUpdateSpoonCodeAction;

namespace Cbms.Kms.Application.Orders.Commands
{
    public class OrderUpdateSpoonCodeCommand : CommandBase<OrderUpdateSpoonResult>
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string QrCode { get; set; }
        public string SpoonCode { get; set; }
    }
}
