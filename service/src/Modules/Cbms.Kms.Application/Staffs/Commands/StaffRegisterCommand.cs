using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Staffs.Commands
{
    public class StaffRegisterCommand : CommandBase
    {
        public StaffRegisterDto Data { get; set; }
    }
}