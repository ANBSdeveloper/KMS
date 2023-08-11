using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationSendAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public NotificationSendAction(IIocResolver iocResolver, ILocalizationSource localizationSource)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
        }
    }
}
