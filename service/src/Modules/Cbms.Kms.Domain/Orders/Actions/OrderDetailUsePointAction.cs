using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Orders.Actions
{
    public class OrderDetailUsePointAction : IEntityAction
    {
        public OrderDetailUsePointAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            decimal usePoints)
        {
            UsePoints = usePoints;
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
        }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public decimal UsePoints { get; private set; }
    }
}