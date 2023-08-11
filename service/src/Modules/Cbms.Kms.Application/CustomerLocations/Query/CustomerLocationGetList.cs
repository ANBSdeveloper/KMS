using Cbms.Kms.Application.CustomerLocations.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.CustomerLocations.Query
{
    public class CustomerLocationGetList : EntityPagingResultQuery<CustomerLocationDto>
    {
        public bool? IsActive { get; set; }
    }
}
