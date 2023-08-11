using Cbms.Mediator;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Customers.Commands
{
    public class CustomerRefuseKeyShopCommand : CommandBase
    {
        public List<int> Data { get; set; }
    }
}