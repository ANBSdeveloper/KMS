using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetByCode : QueryBase, IRequest<CustomerDto>
    {
        public CustomerGetByCode(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}