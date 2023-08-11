using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.Orders.Actions
{
    public class OrderDetailCreateAction : IEntityAction
    {
        public OrderDetailCreateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int branchId,
            string compareProductCode,
            string qrCode,
            string spoonCode,
            bool checkSpoon,
            bool updatePoint,
            string api)
        {
            BranchId = branchId;
            CompareProductCode = compareProductCode;
            QrCode = qrCode;
            SpoonCode = spoonCode;
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            CheckSpoon = checkSpoon;
            UpdatePoint = updatePoint;
            Api = api;
        }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int BranchId { get; private set; }
        public string CompareProductCode { get; private set; }
        public string QrCode { get; private set; }
        public string SpoonCode { get; private set; }
        public bool CheckSpoon { get; private set; }
        public bool UpdatePoint { get; private set; }
        public string Api { get; private set; }
    }
}