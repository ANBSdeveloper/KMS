using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketProgressUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int UserId { get; private set; }
        public int ProgressId { get; private set; }
        public string DocumentPhoto1 { get; private set; }
        public string DocumentPhoto2 { get; private set; }
        public string DocumentPhoto3 { get; private set; }
        public string DocumentPhoto4 { get; private set; }
        public string DocumentPhoto5 { get; private set; }
        public string Note { get; private set; }
        public List<RewardItem> UpsertRewardItems { get; private set; }
        public List<Material> UpsertMaterials { get; private set; }
        public List<TicketRewardItem> RegisterRewardItems { get; private set; }
        public List<TicketMaterial> RegisterMaterials { get; private set; }

        public TicketProgressUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            int progressId,
            string documentPhoto1,
            string documentPhoto2,
            string documentPhoto3,
            string documentPhoto4,
            string documentPhoto5,
            string note,
            List<RewardItem> upsertRewardItemUpdates,
            List<Material> upsertMaterialUpdates,
            List<TicketRewardItem> registerRewardItems,
            List<TicketMaterial> registerMaterials
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            ProgressId = progressId;
            Note = note;
            DocumentPhoto1 = documentPhoto1;
            DocumentPhoto2 = documentPhoto2;
            DocumentPhoto3 = documentPhoto3;
            DocumentPhoto4 = documentPhoto4;
            DocumentPhoto5 = documentPhoto5;
            UpsertRewardItems = upsertRewardItemUpdates;
            UpsertMaterials = upsertMaterialUpdates;
            RegisterRewardItems = registerRewardItems;
            RegisterMaterials = registerMaterials;
        }

        public class RewardItem
        {
            public int Id { get; private set; }
            public int RewardItemId { get; private set; }
            public bool IsReceived { get; private set; }
            public RewardItem(int id, int rewardItemId, bool isReceived)
            {
                Id = id;
                RewardItemId = rewardItemId;
                IsReceived = isReceived;
            }
        }

        public class Material
        {
            public int Id { get; private set; }
            public int MaterialId { get; private set; }
            public bool IsReceived { get; private set; }
            public bool IsSentDesign { get; private set; }
            public string Photo1 { get; private set; }
            public string Photo2 { get; private set; }
            public string Photo3 { get; private set; }
            public string Photo4 { get; private set; }
            public string Photo5 { get; private set; }

            public Material(int id, int materialId, bool isReceived, bool isSentDesign, string photo1, string photo2, string photo3, string photo4, string photo5)
            {
                Id = id;
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
}