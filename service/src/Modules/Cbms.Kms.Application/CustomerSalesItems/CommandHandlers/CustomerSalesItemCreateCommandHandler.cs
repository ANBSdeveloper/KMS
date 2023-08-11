using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.CustomerSalesItems.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.CustomerSalesItems;
using Cbms.Kms.Domain.CustomerSalesItems.Actions;
using Cbms.Kms.Domain.Integration;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.CommandHandlers
{
    public class CustomerSalesItemCreateCommandHandler : RequestHandlerBase, IRequestHandler<CustomerSalesItemCreateCommand, string>
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<CustomerSalesItem, int> _customerSalesItemRepository;
        private readonly DistributedLockManager _distributedLockManager;
        private readonly IProductManager _productManager;
        private readonly IRewardAppManager _rewardAppManager;
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        public CustomerSalesItemCreateCommandHandler(
            DistributedLockManager distributedLockManager, 
            IRequestSupplement supplement,
            IRepository<Customer, int> customerRepository,
            IRepository<CustomerSalesItem, int> customerSalesItemRepository,
            IProductManager productManager,
            IRewardAppManager rewardAppManager,
            IRepository<TicketInvestment, int> ticketInvestmentRepository
        ) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _customerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
            _customerSalesItemRepository = customerSalesItemRepository;
            _productManager = productManager;
            _rewardAppManager = rewardAppManager;
            _ticketInvestmentRepository = ticketInvestmentRepository;
        }

        public async Task<string> Handle(CustomerSalesItemCreateCommand request, CancellationToken cancellationToken)
        {
            await using (await _distributedLockManager.AcquireAsync($"order"))
            {
                var customer = await _customerRepository.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (customer == null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.UserIsNotCustomer").Build();
                }

                var productInfo = await _productManager.CheckAndGetInfoByQrCodeAsync(customer.BranchId.Value, request.Data.QrCode, false);

                CustomerSalesItem salesItem = null;
                if (productInfo.ChildQrCodes.Count > 0)
                {
                    foreach (var qrCode in productInfo.ChildQrCodes)
                    {
                        salesItem = await AddSalesItemAsync(customer.Id, productInfo.ProductId, qrCode);
                    }
                } else
                {
                    salesItem = await AddSalesItemAsync(customer.Id, productInfo.ProductId, productInfo.QrCode);
                }


                await _customerSalesItemRepository.UnitOfWork.CommitAsync();

                var ticketInvestment = await _ticketInvestmentRepository
                    .GetAllIncluding(p => p.Tickets)
                    .FirstOrDefaultAsync(p => p.Id == salesItem.TicketInvestmentId);

                if (ticketInvestment.OutOfTicket())
                {
                    return LocalizationSource.GetString("TicketInvestment.OutOfTicket");
                }
                await _rewardAppManager.SyncQrCode(
                        customer.Code,
                        salesItem.CreationTime,
                        ticketInvestment.IssueTicketBeginDate,
                        ticketInvestment.IssueTicketEndDate,
                        productInfo.ChildQrCodes.Count > 0 ? productInfo.ChildQrCodes : new List<string>() { productInfo.QrCode });
                return string.Empty;
            }
        }

        private async Task<CustomerSalesItem> AddSalesItemAsync(int customerId, int productId, string qrCode)
        {
            var salesItem = new CustomerSalesItem();
            await salesItem.ApplyActionAsync(new CustomerSalesItemCreateAction(
                IocResolver,
                LocalizationSource,
                customerId,
                productId,   
                qrCode
            ));

            await _customerSalesItemRepository.InsertAsync(salesItem);

            return salesItem;
        }
    }
}