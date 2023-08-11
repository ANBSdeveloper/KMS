using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.CustomerSalesItems.Actions;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Timing;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.CustomerSalesItems
{
    public class CustomerSalesItem : AuditedAggregateRoot
    {
        public int CustomerId { get; private set; }
        public string QrCode { get; private set; }
        public int ProductId { get; private set; }
        public decimal Price { get; private set; }
        public bool IsUsing { get; private set; }
        public int TicketInvestmentId { get; private set; }
        public CustomerSalesItem()
        {
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case CustomerSalesItemCreateAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case CustomerSalesItemSetUsingAction setUsingAction:
                    await SetUsingAsync(setUsingAction);
                    break;
            }
        }

        public async Task UpsertAsync(CustomerSalesItemCreateAction action)
        {
  
            var ticketInvestmentManager = action.IocResolver.Resolve<ITicketInvestmentManager>();

            var ticketInvestment = await ticketInvestmentManager.GetActiveTicketInvestmentAsync(action.CustomerId, Clock.Now.Date);

            if (ticketInvestment == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.NoActiveTicketInvestment").Build();
            }

            var settingManager = action.IocResolver.Resolve<IAppSettingManager>();
            bool sampleTicketMode = await settingManager.IsEnableAsync("TICKET_SAMPLE_MODE");
            if (!ticketInvestment.ValidIssueTime() && !sampleTicketMode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode(
                        "TicketInvestment.CreateOrderIssueDateInvalid",
                        ticketInvestment.IssueTicketBeginDate.ToShortDateString(),
                        ticketInvestment.IssueTicketEndDate.ToShortDateString()
                    ).Build();
            }


            if (!ticketInvestment.IsInIssueStage())
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource)
                    .MessageCode(
                        "TicketInvestment.EndIssueStage",
                        ticketInvestment.Code
                    ).Build();
            }

            // Note: cho phép bán lại ở shop khác
            var repository = action.IocResolver.Resolve<IRepository<CustomerSalesItem, int>>();
            var existsSalesItem = await repository.FirstOrDefaultAsync(p => p.QrCode == action.QrCode);
            if (existsSalesItem != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.QrCodeIsUsed", action.QrCode).Build();
            }

            CustomerId = action.CustomerId;
            QrCode = action.QrCode;
            ProductId = action.ProductId;
            TicketInvestmentId = ticketInvestment.Id;

            var priceManager = action.IocResolver.Resolve<IProductManager>();
            Price = await priceManager.GetPriceAsync(action.ProductId);
        }

        public async Task SetUsingAsync(CustomerSalesItemSetUsingAction action)
        {
            IsUsing = true;
        }
    }
}