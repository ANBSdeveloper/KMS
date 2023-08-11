using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGet : EntityQuery<CustomerDto>
    {
        public CustomerGet(int id) : base(id)
        {
        }
    }
}