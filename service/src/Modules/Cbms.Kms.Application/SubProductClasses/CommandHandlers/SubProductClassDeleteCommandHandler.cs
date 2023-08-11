using Cbms.Kms.Application.SubProductClasses.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Mediator;

namespace Cbms.Kms.Application.SubProductClasses.CommandHandlers
{
    public class SubProductClassDeleteCommandHandler : DeleteEntityCommandHandler<SubProductClassDeleteCommand, SubProductClass>
    {
        public SubProductClassDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }
    }
}