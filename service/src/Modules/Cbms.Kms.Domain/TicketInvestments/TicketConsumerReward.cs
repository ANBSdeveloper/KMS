using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketConsumerReward : AuditedEntity
    {
        public int RewardItemId { get; private set; }
        public int Quantity { get; private set; }
        public int RewardQuantity { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public IReadOnlyCollection<TicketConsumerRewardDetail> Details => _details;
        public List<TicketConsumerRewardDetail> _details = new List<TicketConsumerRewardDetail>();

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketConsumerRewardUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(TicketConsumerRewardUpsertAction action)
        {
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            Quantity = action.Quantity;
            RewardItemId = action.RewardItemId;
            Photo1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : Photo1;
            Photo2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : Photo2;
            Photo3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : Photo3;
            Photo4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : Photo4;
            Photo5 = action.Photo5 != Photo5 ? await imageResizer.ResizeBase64Image(action.Photo5) : Photo5;

            foreach (var id in action.DeleteRewardDetails)
            {
                var rewardDetail = _details.FirstOrDefault(p => p.Id == id);
                if (rewardDetail != null)
                {
                    _details.Remove(rewardDetail);
                }
            }

            foreach (var item in action.UpsertRewardDetails)
            {
                TicketConsumerRewardDetail consumerRewardDetail;
                if (!item.Id.IsNew())
                {
                    consumerRewardDetail = _details.FirstOrDefault(p => p.Id == item.Id);
                    if (consumerRewardDetail == null)
                    {
                        throw new EntityNotFoundException(typeof(TicketConsumerRewardDetail), item.Id);
                    }
                }
                else
                {
                    consumerRewardDetail = new TicketConsumerRewardDetail();
                    _details.Add(consumerRewardDetail);
                }

                var existsTicket = action.IssueTickets.FirstOrDefault(p => p.Id == item.TicketId);
                if (existsTicket == null)
                {
                    var ticketRespository = action.IocResolver.Resolve<IRepository<Ticket, int>>();
                    var ticket = await ticketRespository.GetAsync(item.TicketId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.TicketNotExists", ticket.Code).Build();
                }

                await consumerRewardDetail.ApplyActionAsync(new TicketConsumerRewardDetailUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.TicketId,
                    item.Note));
            }

            RewardQuantity = _details.Count;
        }
    }
}