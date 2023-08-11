using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketProgress : AuditedEntity
    {
        public DateTime UpdateTime { get; private set; }
        public string DocumentPhoto1 { get; private set; }
        public string DocumentPhoto2 { get; private set; }
        public string DocumentPhoto3 { get; private set; }
        public string DocumentPhoto4 { get; private set; }
        public string DocumentPhoto5 { get; private set; }
        public string Note { get; private set; }
        public IReadOnlyCollection<TicketProgressMaterial> Materials => _materials;
        public List<TicketProgressMaterial> _materials = new List<TicketProgressMaterial>();

        public IReadOnlyCollection<TicketProgressRewardItem> RewardItems => _rewardItems;
        public List<TicketProgressRewardItem> _rewardItems = new List<TicketProgressRewardItem>();
        public int UpdateUserId { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketProgressUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(TicketProgressUpsertAction action)
        {
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            UpdateUserId = action.UserId;
            UpdateTime = Clock.Now;
            Note = action.Note ?? "";
            DocumentPhoto1 = action.DocumentPhoto1 != DocumentPhoto1 ? await imageResizer.ResizeBase64Image(action.DocumentPhoto1) : DocumentPhoto1;
            DocumentPhoto2 = action.DocumentPhoto2 != DocumentPhoto2 ? await imageResizer.ResizeBase64Image(action.DocumentPhoto2) : DocumentPhoto2;
            DocumentPhoto3 = action.DocumentPhoto3 != DocumentPhoto3 ? await imageResizer.ResizeBase64Image(action.DocumentPhoto3) : DocumentPhoto3;
            DocumentPhoto4 = action.DocumentPhoto4 != DocumentPhoto4 ? await imageResizer.ResizeBase64Image(action.DocumentPhoto4) : DocumentPhoto4;
            DocumentPhoto5 = action.DocumentPhoto5 != DocumentPhoto5 ? await imageResizer.ResizeBase64Image(action.DocumentPhoto5) : DocumentPhoto5;

            foreach (var item in action.UpsertRewardItems)
            {
                TicketProgressRewardItem rewardItemUpdate;
                if (!item.Id.IsNew())
                {
                    rewardItemUpdate = _rewardItems.FirstOrDefault(p => p.Id == item.Id);
                    if (rewardItemUpdate == null)
                    {
                        throw new EntityNotFoundException(typeof(TicketProgress), item.Id);
                    }
                }
                else
                {
                    rewardItemUpdate = new TicketProgressRewardItem();
                    _rewardItems.Add(rewardItemUpdate);
                }

                var existsRewardItem = action.RegisterRewardItems.FirstOrDefault(p => p.RewardItemId == item.RewardItemId);
                if (existsRewardItem == null)
                {
                    var rewardItemRespository = action.IocResolver.Resolve<IRepository<RewardItem, int>>();
                    var rewardItem = await rewardItemRespository.GetAsync(item.RewardItemId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RewardItemNotRegister", rewardItem.Name).Build();
                }

                await rewardItemUpdate.ApplyActionAsync(new TicketProgressRewardItemUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.RewardItemId,
                    item.IsReceived));
            }

            foreach (var item in action.UpsertMaterials)
            {
                TicketProgressMaterial materialUpdate;
                if (!item.Id.IsNew())
                {
                    materialUpdate = _materials.FirstOrDefault(p => p.Id == item.Id);
                    if (materialUpdate == null)
                    {
                        throw new EntityNotFoundException(typeof(TicketProgressMaterial), item.Id);
                    }
                }
                else
                {
                    materialUpdate = new TicketProgressMaterial();
                    _materials.Add(materialUpdate);
                }

                var existsMaterial = action.RegisterMaterials.FirstOrDefault(p => p.MaterialId == item.MaterialId);
                if (existsMaterial == null)
                {
                    var rewardItemRespository = action.IocResolver.Resolve<IRepository<RewardItem, int>>();
                    var rewardItem = await rewardItemRespository.GetAsync(item.MaterialId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.MaterialNotRegister", rewardItem.Name).Build();
                }

                await materialUpdate.ApplyActionAsync(new TicketProgressMaterialUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.MaterialId,
                    item.IsReceived,
                    item.IsSentDesign,
                    item.Photo1,
                    item.Photo2,
                    item.Photo3,
                    item.Photo4,
                    item.Photo5));
            }
        }
    }
}