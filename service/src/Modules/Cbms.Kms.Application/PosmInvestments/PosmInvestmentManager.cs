using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Cbms.Kms.Application.TicketInvestments.TicketInvestmentManager;

namespace Cbms.Kms.Application.PosmInvestments
{
    public class PosmInvestmentManager : IPosmInvestmentManager, ITransientDependency
    {
        private readonly AppDbContext _dbContext;
        private readonly IIocResolver _iocResolver;
        private readonly ILocalizationManager _localizationManager;
        private readonly IRepository<PosmInvestment, int> _ticketInvestmentRepository;
        public PosmInvestmentManager(
            AppDbContext dbContext,
            IRepository<PosmInvestment, int> ticketInvestmentRepository,
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

            var investments = from p in _dbContext.PosmInvestments
                              join c in _dbContext.Cycles on p.CycleId equals c.Id
                              where p.CustomerId == customerId && c.Year == DateTime.Now.Year
                              select p;
            var nextNumber = (investments.Count() + 1).ToString();
            nextNumber = ("000" + nextNumber);
            nextNumber = nextNumber.Substring(nextNumber.Length - 3, 3);
            string code = customer.Code + "_" + DateTime.Now.Year.ToString().Substring(2, 2) + nextNumber;

            return code;
        }
        public async Task<string> GetHistoryDataAsync(PosmInvestmentItem item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            var temp = JsonConvert.DeserializeObject<PosmInvestmentItem>(jsonString, new JsonSerializerSettings()
            {
                ContractResolver = new PrivateResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

            await temp.ApplyActionAsync(new PosmInvestmentItemClearAction());

            return JsonConvert.SerializeObject(temp);
        }

        public async Task<decimal> GetPriceAsync(int posmItemId)
        {
            var today = DateTime.Now.Date;
            var lastestDetail = await (from detail in _dbContext.PosmPriceDetails
                                      join header in _dbContext.PosmPriceeHeaders on detail.PosmPriceHeaderId equals header.Id
                                      where detail.PosmItemId == posmItemId && header.FromDate <= today && header.ToDate >= today && header.IsActive
                                      orderby header.ToDate descending
                                      select detail).FirstOrDefaultAsync();

            return lastestDetail != null ? lastestDetail.Price : 0;

        }
    }
}