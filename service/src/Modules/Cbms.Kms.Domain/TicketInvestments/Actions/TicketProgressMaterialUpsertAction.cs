using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketProgressMaterialUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int MaterialId { get; private set; }
        public bool IsReceived { get; private set; }
        public bool IsSentDesign { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }

        public TicketProgressMaterialUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int materialId,
            bool isReceived,
            bool isSentDesign,
            string photo1,
            string photo2,
            string photo3,
            string photo4,
            string photo5
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            MaterialId = materialId;
            IsReceived = isReceived;
            IsSentDesign = isSentDesign;
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
            Photo5 = photo5;
        }
    }
}