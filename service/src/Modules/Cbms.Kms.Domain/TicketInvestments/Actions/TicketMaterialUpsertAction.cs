using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketMaterialUpsertAction : IEntityAction
    {
        public int MaterialId { get; private set; }
        public int RegisterQuantity { get; private set; }
        public decimal Price { get; private set; }
        public string Note { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketMaterialUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int materialId,
            int registerQuantity,
            decimal price,
            string note
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            MaterialId = materialId;
            RegisterQuantity = registerQuantity;
            Price = price;
            Note = note ?? "";
        }
    }
}