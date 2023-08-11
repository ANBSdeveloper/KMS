using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetQrData : QueryBase, IRequest<string>
    {
        public CustomerGetQrData(string code)
        {
            Code = code;
        }
        public string Code { get; private set; }
    }
}