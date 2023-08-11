using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.Customers.Query
{
    public class ConsumerGetByPhone : QueryBase, IRequest<ConsumerInfoDto>
    {
        public string Phone { get; set; }
    }
}