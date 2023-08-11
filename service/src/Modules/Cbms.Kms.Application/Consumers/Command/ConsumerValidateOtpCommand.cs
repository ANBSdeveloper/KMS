using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Consumers.Commands
{
    public class ConsumerValidateOtpCommand : CommandBase
    {
        public ConsumerValidateOtpDto Data { get; set; }
    }
}