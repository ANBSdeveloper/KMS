using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerLocations.Query
{
    public class CustomerLocationGet : EntityQuery<CustomerLocationDto>
    {
        public CustomerLocationGet(int id) : base(id)
        {
        }
    }
}
