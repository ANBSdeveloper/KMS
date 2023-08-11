using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;

namespace Cbms.Application.Staffs.Commands
{
    public class StaffUpdateCreditPointCommand : CommandBase
    {
        public StaffUpdateCreditPointDto Data { get; set; }

        public StaffUpdateCreditPointCommand WithId(int id)
        {
            Data.StaffId = id;
            return this;
        } 
    }
}