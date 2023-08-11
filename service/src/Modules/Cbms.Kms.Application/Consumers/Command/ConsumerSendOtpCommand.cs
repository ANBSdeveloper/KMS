using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Consumers.Commands
{
    public class ConsumerSendOtpCommand : CommandBase
    {
        public ConsumerSendOtpDto Data { get; set; }
    }
}