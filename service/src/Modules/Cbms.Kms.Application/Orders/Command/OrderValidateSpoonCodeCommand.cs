using Cbms.Mediator;
namespace Cbms.Kms.Application.Orders.Commands
{
    public class OrderValidateSpoonCodeCommand : CommandBase
    {
        public string SpoonCode { get; set; }
    }
}
