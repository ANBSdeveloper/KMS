using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Orders.Commands;
using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Integration;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.Orders.Actions;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.CommandHandlers
{
    public class OrderCreateCommandHandler : RequestHandlerBase, IRequestHandler<OrderCreateCommand, OrderDto>
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly DistributedLockManager _distributedLockManager;
        private readonly IRewardAppManager _rewardAppManager;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly IAppSettingManager _appSettingManager;
        public OrderCreateCommandHandler(
            DistributedLockManager distributedLockManager, 
            IRequestSupplement supplement, 
            IRepository<Order, int> orderRepository,
            IRewardAppManager rewardAppManager,
            IRepository<Customer, int> customerRepository,
            IRepository<TicketInvestment, int> ticketInvestmentRepository,
            IAppSettingManager appSettingManager) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _orderRepository = orderRepository;
            _distributedLockManager = distributedLockManager;
            _rewardAppManager = rewardAppManager;
            _customerRepository = customerRepository;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _appSettingManager = appSettingManager;
        }

        public async Task<OrderDto> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            if (!await _appSettingManager.IsEnableAsync("ENABLE_CREATE_TICKET_FROM_SHOP"))
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource)
                    .MessageCode("Order.NotAllowCreateTicketFromShop")
                    .Build();
            }

            var order = new Order();

            await using (await _distributedLockManager.AcquireAsync($"order"))
            {

                await order.ApplyActionAsync(new OrderCreateAction(
                    IocResolver,
                    LocalizationSource,
                    Session.UserId.Value,
                    requestData.ConsumerPhone,
                    requestData.ConsumerName,
                    true,
                    "KMS",
                    requestData.OrderDetails.Select(p => new OrderCreateAction.OrderDetail(
                       p.ProductCode,
                       p.QrCode,
                       p.SpoonCode
                    )).ToList(),
                    null
                ));

                await _orderRepository.InsertAsync(order);

                await _orderRepository.UnitOfWork.CommitAsync(cancellationToken);

                var customer = await _customerRepository.GetAsync(order.CustomerId);
                var ticketInvestment = await _ticketInvestmentRepository.GetAsync(order.TicketInvestmentId.Value);
                await _rewardAppManager.SyncQrCode(
                    customer.Code,
                    order.CreationTime,
                    ticketInvestment.IssueTicketBeginDate,
                    ticketInvestment.IssueTicketEndDate,
                    order.OrderDetails.Select(p=>p.QrCode).Distinct().ToList()
                );

                return await Mediator.Send(new OrderGet(order.Id));
            }
            
        }
    }
}