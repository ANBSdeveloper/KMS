using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Kms.Infrastructure;
using Cbms.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments
{
    public class TicketInvestmentManager : ITicketInvestmentManager, ITransientDependency
    {
        private readonly AppDbContext _dbContext;
        private readonly IIocResolver _iocResolver;
        private readonly ILocalizationManager _localizationManager;
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        public TicketInvestmentManager(
            AppDbContext dbContext,
            IRepository<TicketInvestment, int> ticketInvestmentRepository,
            IIocResolver iocResolver,
            ILocalizationManager localizationManager)
        {
            _dbContext = dbContext;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _iocResolver = iocResolver;
            _localizationManager = localizationManager;
        }

        public async Task<string> GenerateCodeAsync(int customerId)
        {
            var customer = _dbContext.Customers.FirstOrDefault(p => p.Id == customerId);

            var investments = from p in _dbContext.TicketInvestments
                              join c in _dbContext.Cycles on p.CycleId equals c.Id
                              where p.CustomerId == customerId && c.Year == DateTime.Now.Year
                              select p;
            var nextNumber = (investments.Count() + 1).ToString();
            nextNumber = ("000" + nextNumber);
            nextNumber = nextNumber.Substring(nextNumber.Length - 3, 3);
            string code = customer.Code + "_" + DateTime.Now.Year.ToString().Substring(2, 2) + nextNumber;

            return code;
        }

        public async Task<Ticket> GenerateTicketAsync(int ticketInvestmentId, string consumerPhone, string consumerName)
        {
            var investment = await _ticketInvestmentRepository
                .GetAllIncluding(p => p.Tickets)
                .FirstOrDefaultAsync(p => p.Id == ticketInvestmentId);

            Ticket ticket = null;

            await investment.ApplyActionAsync(new TicketGenerateAction(
                _iocResolver,
                _localizationManager.GetDefaultSource(),
                consumerPhone,
                consumerName,
                (result) => ticket = result)
            );

            await _ticketInvestmentRepository.UnitOfWork.CommitAsync();

            return ticket;
        }

        public async Task<TicketInvestment> GetActiveTicketInvestmentAsync(int customerId, DateTime validDate)
        {
            var ticketInvestment = (from p in _dbContext.TicketInvestments.Include(p => p.Tickets)
                                    join c in _dbContext.Cycles on p.CycleId equals c.Id
                                    where c.FromDate <= validDate
                                    //c.ToDate >= validDate
                                    && p.CustomerId == customerId
                                    && c.IsActive
                                    && (p.Status == TicketInvestmentStatus.Approved || p.Status == TicketInvestmentStatus.Doing)
                                    select p).FirstOrDefault();
            return ticketInvestment;
        }

        public async Task<string> GetHistoryDataAsync(TicketInvestment ticketInvestment)
        {
            var jsonString = JsonConvert.SerializeObject(ticketInvestment);
            var temp = JsonConvert.DeserializeObject<TicketInvestment>(jsonString, new JsonSerializerSettings()
            {
                ContractResolver = new PrivateResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

            await temp.ApplyActionAsync(new TicketInvestmentClearAction());

            return JsonConvert.SerializeObject(temp);
        }

        public class PrivateResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    var hasPrivateSetter = property?.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
                return prop;
            }
        }
    }
}