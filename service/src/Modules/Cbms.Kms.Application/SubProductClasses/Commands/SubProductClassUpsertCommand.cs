using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.SubProductClasses.Commands
{
    public class SubProductClassUpsertCommand : UpsertEntityCommand<SubProductClassUpsertDto, SubProductClassDto>
    {
        public SubProductClassUpsertCommand(SubProductClassUpsertDto data, string handleType) : base(data, handleType)
        {
        }
    }
}