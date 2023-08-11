using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Timing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class Ticket : AggregateRoot
    {
        public string Code { get; private set; }
        public string ConsumerPhone { get; private set; }
        public string ConsumerName { get; private set; }
        public DateTime? IssueDate { get; private set; }
        public DateTime? PrintDate { get; private set; }
        public int PrintCount { get; private set; }
        public int? LastPrintUserId { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketCreateAction createAction:
                    await CreateAsync(createAction);
                    break;
                case TicketUpdateAction createAction:
                    await UpdateAsync(createAction);
                    break;
            }
        }

        private async Task CreateAsync(TicketCreateAction action) 
        {
            var ticketRepository = action.IocResolver.Resolve < IRepository<Ticket, int>>();
            bool isDuplicate = false;
            do
            {
                Code = GetRandomCode(6);
                isDuplicate = ticketRepository.GetAll().Where(p => p.Code == Code && p.TicketInvestmentId == action.TickteInvestmentId).Any();

            } while (isDuplicate);
          
            ConsumerPhone = action.ConsumerPhone;
            ConsumerName = action.ConsumerName;
            IssueDate = Clock.Now.Date;
        }

        private string GetRandomCode(int length)
        {
            var allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public async Task UpdateAsync(TicketUpdateAction action)
        {
            PrintDate = Clock.Now.Date;
            PrintCount ++;
            LastPrintUserId = action.StaffId;
        }
    }
}