using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Notifications.Actions
{
    public class NotificationDeleteAction : IEntityAction
    {
        public ILocalizationSource LocalizationSource { get; private set; }
        public NotificationDeleteAction(ILocalizationSource localizationSource)
        {
            LocalizationSource = localizationSource;
        }
    }
}
