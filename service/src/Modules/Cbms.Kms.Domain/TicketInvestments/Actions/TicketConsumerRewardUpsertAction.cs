using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketConsumerRewardUpsertAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int UserId { get; private set; }
        public int RewardItemId { get; private set; }
        public int Quantity { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public List<ConsumerRewardDetail> UpsertRewardDetails { get; private set; }
        public List<int> DeleteRewardDetails { get; private set; }
        public List<Ticket> IssueTickets { get; private set; }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }
        public TicketConsumerRewardUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            int rewardItemId,
            int quantity,
            string photo1,
            string photo2,
            string photo3,
            string photo4,
            string photo5,
            List<ConsumerRewardDetail> upsertRewardItems,
            List<int> deleteRewardItems,
            List<Ticket> issueTickets
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            RewardItemId = rewardItemId;
            Quantity = quantity;
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
            Photo5 = photo5;
            UpsertRewardDetails = upsertRewardItems;
            DeleteRewardDetails = deleteRewardItems;
            IssueTickets = issueTickets;
        }

        public class ConsumerRewardDetail
        {
            public int Id { get; private set; }
            public int TicketId { get; private set; }
            public string Note { get; private set; }

            public ConsumerRewardDetail(int id, int ticketId, string note)
            {
                Id = id;
                TicketId = ticketId;
                Note = note;
            }
        }
    }
}