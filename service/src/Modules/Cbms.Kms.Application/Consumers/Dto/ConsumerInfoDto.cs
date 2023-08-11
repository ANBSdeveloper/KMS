using AutoMapper;
using Cbms.Kms.Domain.Consumers;

namespace Cbms.Kms.Application.Consumers.Dto
{
    [AutoMap(typeof(ConsumerInfo))]
    public class ConsumerInfoDto: ConsumerInfo
    {
    }
}
