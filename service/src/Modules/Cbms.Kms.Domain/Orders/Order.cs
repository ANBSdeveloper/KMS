using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Consumers;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Kms.Domain.CustomerSalesItems;
using Cbms.Kms.Domain.CustomerSalesItems.Actions;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Orders.Actions;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Localization.Sources;
using Cbms.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cbms.Kms.Domain.Orders.Actions.OrderUpdateSpoonCodeAction;

namespace Cbms.Kms.Domain.Orders
{
    public class Order : AuditedAggregateRoot
    {
        public List<OrderDetail> _orderDetails = new List<OrderDetail>();
        public List<OrderTicket> _orderTickets = new List<OrderTicket>();
        public int BranchId { get; private set; }
        public string ConsumerName { get; private set; }
        public string ConsumerPhone { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public IReadOnlyCollection<OrderDetail> OrderDetails => _orderDetails;
        public string OrderNumber { get; private set; }
        public IReadOnlyCollection<OrderTicket> OrderTickets => _orderTickets;
        public int? TicketInvestmentId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal TotalAvailablePoints { get; private set; }
        public decimal TotalPoints { get; private set; }
        public decimal TotalQuantity { get; private set; }
        public decimal TotalUsedPoints { get; private set; }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case OrderCreateAction createAction:
                    await CreateAsync(createAction);
                    break;

                case OrderAddTicketAction addTicketAction:
                    await AddTicketAsync(addTicketAction);
                    break;

                case OrderUpdateSpoonCodeAction updateSpoonAction:
                    await UpdateSpoonAsync(updateSpoonAction);
                    break;
            }
        }

        public async Task CreateAsync(OrderCreateAction action)
        {
            var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();
            var customer = customerRepository.GetAll().FirstOrDefault(p => p.UserId == action.UserId);
            if (customer == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.NotValidCreateOrder").Build();
            }

            if (string.IsNullOrEmpty(action.ConsumerName))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("ConsumerName").Build();
            }

            if (string.IsNullOrEmpty(action.ConsumerPhone))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Phone").Build();
            }

            if (action.OrderDetails.Count == 0)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.MustAtLeastOneItem").Build();
            }

            var duplicateQrCode = action.OrderDetails.GroupBy(p => p.QrCode).Where(p => p.Count() > 1).FirstOrDefault();
            if (duplicateQrCode != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DuplicateQrCode", duplicateQrCode.Key).Build();
            }

            var spoonCode = action.OrderDetails.GroupBy(p => p.SpoonCode).Where(p => p.Count() > 1).FirstOrDefault();
            if (spoonCode != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DuplicateSpoonCode", spoonCode.Key).Build();
            }

            BranchId = customer.BranchId.Value;
            ConsumerName = action.ConsumerName;
            ConsumerPhone = action.ConsumerPhone;
            CustomerId = customer.Id;
            OrderDate = Clock.Now;
            OrderNumber = $"{customer.Code}_{Clock.Now:yyMMddHHmmss}";

            var ticketInvestmentManager = action.IocResolver.Resolve<ITicketInvestmentManager>();

            var ticketInvestment = await ticketInvestmentManager.GetActiveTicketInvestmentAsync(customer.Id, Clock.Now.Date);
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


            TicketInvestmentId = ticketInvestment.Id;

            var ticketRepository = action.IocResolver.Resolve<IRepository<Ticket, int>>();
            var issueTicketQuantity = ticketRepository.GetAll().Where(p => p.TicketInvestmentId == ticketInvestment.Id).Count();

            bool outOfTicket = issueTicketQuantity >= ticketInvestment.TicketQuantity;

            foreach (var item in action.OrderDetails)
            {
                var orderDetail = new OrderDetail();
                await orderDetail.ApplyActionAsync(new OrderDetailCreateAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    BranchId,
                    item.CompareProductCode,
                    item.QrCode,
                    item.SpoonCode,
                    action.CheckSpoon,
                    !outOfTicket,
                    action.Api));
                _orderDetails.Add(orderDetail);
            }
            CalcTotal();

            (var actualNewTickets,int  newTicketQuantity) = await CalcTicketAsync(action.IocResolver, action.LocalizationSource, ticketInvestment.PointsForTicket, ticketInvestment.TicketQuantity, issueTicketQuantity);

            if (actualNewTickets.Count > 0 && action.Api == "KMS")
            {
                await SendSmsAsync(action.IocResolver, action.LocalizationSource, customer, actualNewTickets, newTicketQuantity);
            }
            var consumerRepository = action.IocResolver.Resolve<IRepository<Consumer, int>>();
            var consumer = await consumerRepository.FirstOrDefaultAsync(p => p.Phone == action.ConsumerPhone);
            if (consumer != null)
            {
                await consumer.ApplyActionAsync(new ConsumerCreateAction(action.ConsumerPhone, action.ConsumerName));
            }

            if (action.GetResult != null)
            {
                action.GetResult(new OrderCreateAction.OrderCreateResult()
                {
                    ActualNewTickets = actualNewTickets,
                    OutOfTicket = actualNewTickets.Count < newTicketQuantity
                });
            }
        }

        public async Task UpdateSpoonAsync(OrderUpdateSpoonCodeAction action)
        {
            var appLogger = action.IocResolver.Resolve<IAppLogger>();
            try
            {
                if (string.IsNullOrEmpty(action.QrCode))
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.QrCodeInvalid").Build();
                }
                if (string.IsNullOrEmpty(action.SpoonCode))
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.SpoonCodeInvalid").Build();
                }

                var customerSalesItemRepository = action.IocResolver.Resolve<IRepository<CustomerSalesItem, int>>();
                var customerSalesItem = customerSalesItemRepository
                       .GetAll()
                       .Where(p => p.QrCode == action.QrCode)
                       .OrderBy(p => p.CreationTime)
                       .FirstOrDefault();

                if (customerSalesItem == null)
                {
                    action.GetResult(new OrderUpdateSpoonResult() { Result = 3 });
                    return;
                }
                var orderDetailRepository = action.IocResolver.Resolve<IRepository<OrderDetail, int>>();

                var orderDetail = orderDetailRepository.FirstOrDefault(p => p.SpoonCode == action.SpoonCode);

                if (orderDetail != null)
                {
                    action.GetResult(new OrderUpdateSpoonResult() { Result = 5 });
                    return;
                }

                orderDetail = orderDetailRepository.FirstOrDefault(p => p.QrCode == action.QrCode);
                if (orderDetail != null)
                {
                    action.GetResult(new OrderUpdateSpoonResult() { Result = 4 });
                    return;
                }
                var ticketInvestmentManager = action.IocResolver.Resolve<ITicketInvestmentManager>();
                var ticketInvestment = await ticketInvestmentManager.GetActiveTicketInvestmentAsync(customerSalesItem.CustomerId, DateTime.Now.Date);

                if (ticketInvestment == null || ticketInvestment.IssueTicketEndDate.Date <= Clock.Now.Date)
                {
                    action.GetResult(new OrderUpdateSpoonResult() { Result = 2 });
                    return;
                }

                var ticketRepository = action.IocResolver.Resolve<IRepository<Ticket, int>>();
                var issueTicketQuantity = ticketRepository.GetAll().Where(p => p.TicketInvestmentId == ticketInvestment.Id).Count();

                bool outOfTicket = issueTicketQuantity >= ticketInvestment.TicketQuantity;

                var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();

                var customer = await customerRepository.GetAsync(customerSalesItem.CustomerId);

                OrderCreateAction.OrderCreateResult createOrderResult = null;

                await ApplyActionAsync(new OrderCreateAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    customer.UserId.Value,
                    action.Phone,
                    action.Name,
                    false,
                    "RA",
                    new List<OrderCreateAction.OrderDetail>() { new OrderCreateAction.OrderDetail("", action.QrCode, action.SpoonCode) },
                    (result) => createOrderResult = result));


                decimal? requiredPoints = null;

                if (!createOrderResult.OutOfTicket && TotalAvailablePoints < ticketInvestment.PointsForTicket)
                {
                    requiredPoints = ticketInvestment.PointsForTicket - TotalAvailablePoints;
                }


                var otherTickets = ticketRepository.GetAll().Where(p => p.TicketInvestmentId == TicketInvestmentId.Value && p.ConsumerPhone == action.Phone).ToList();
                var tickets = new List<OrderUpdateSpoonResultTicket>();
                tickets.AddRange(otherTickets.Select(p => new OrderUpdateSpoonResultTicket() { Code = p.Code, IsNew = false }));
                tickets.AddRange(_orderTickets.Select(p => new OrderUpdateSpoonResultTicket() { Code = p.TicketCode, IsNew = true }));

                var consumerRepository = action.IocResolver.Resolve<IRepository<Consumer, int>>();
                var consumer = await consumerRepository.FirstOrDefaultAsync(p => p.Phone == action.Phone);
                if (consumer == null)
                {
                    consumer = new Consumer();
                    await consumer.ApplyActionAsync(new ConsumerCreateAction(action.Phone, action.Name));
                    await consumerRepository.InsertAsync(consumer);
                }

                await customerSalesItem.ApplyActionAsync(new CustomerSalesItemSetUsingAction(action.IocResolver));

                /*
                   0: Tích điểm BTTT thành công nhưng chưa có mã BTTT.
                   1: Tích điểm thành công & có mã BTTT
                   2: Quá hạn tham gia,
                   3: Mã QR chưa tham gia BTTT trên KMS,
                   4: Mã QR đã được tích điểm BTTT,
                   5: Mã muỗng đã được tích điểm BTTT.
                   6: Shop đã phát đủ phiếu
                   Số điểm tích lũy (nếu có)
                   Số điểm cần tích lũy thêm (nếu có)
                   Thời gian kết thúc tham gia BTTT:(nếu có)
                   Thời gian tổ chức BTTT:(nếu có)
                   Mã Shop tổ chức: (nếu có)
                   Tên Shop tổ chức:(nếu có)
                   Địa chỉ:(nếu có)
                */

                action.GetResult(new OrderUpdateSpoonResult()
                {
                    Result = outOfTicket ? 6 : (_orderTickets.Count > 0 ? 1 : 0),
                    Tickets = tickets,
                    Points = TotalAvailablePoints,
                    ShopCode = customer.Code,
                    ShopName = customer.Name,
                    ShopAddress = customer.Address,
                    IssueTicketEndDate = ticketInvestment.IssueTicketEndDate,
                    OperationDate = ticketInvestment.OperationDate,
                    RequiredPoints = requiredPoints
                });
            }
            catch (BusinessException ex)
            {
                action.GetResult(new OrderUpdateSpoonResult()
                {
                    Result = -1,
                    Message = ex.Message
                });

                await appLogger.LogErrorAsync("UPDATE_SPOON", new
                {
                    Error = ex,
                    Data = new
                    {
                        Phone = action.Phone,
                        Name = action.Name,
                        QrCode = action.QrCode,
                        SpoonCode = action.SpoonCode
                    }
                });
            }
            catch (Exception ex)
            {
                await appLogger.LogErrorAsync("UPDATE_SPOON", new
                {
                    Error = ex,
                    Data = new
                    {
                        Phone = action.Phone,
                        Name = action.Name,
                        QrCode = action.QrCode,
                        SpoonCode = action.SpoonCode
                    }
                });
                throw ex;
            }
        }

        private async Task AddTicketAsync(OrderAddTicketAction action)
        {
            var orderTicket = new OrderTicket();

            await orderTicket.ApplyActionAsync(new OrderTicketCreateAction(
               action.IocResolver,
               action.LocalizationSource,
               action.TicketId,
               action.TicketCode,
               action.QrCode));

            _orderTickets.Add(orderTicket);
        }

        private async Task<(List<Ticket>, int)> CalcTicketAsync(IIocResolver iocResolver, ILocalizationSource localizatonSource, decimal pointsForTicket, int totalTicketQuantity, int issueTicketQuantity)
        {
            var orderRepository = iocResolver.Resolve<IRepository<Order, int>>();
            var recentOrders = orderRepository
                .GetAllIncluding(p => p.OrderDetails, p => p.OrderTickets)
                .Where(p => 
                        p.Id != Id 
                        && p.TotalAvailablePoints > 0 
                        && p.ConsumerPhone == ConsumerPhone 
                        && p.TicketInvestmentId == TicketInvestmentId)
                .OrderBy(p => p.CreationTime).ToList();

            recentOrders.Add(this);

            var totalPoints = recentOrders.Sum(p => p.TotalAvailablePoints);
            List<Ticket> actualNewTickets = new List<Ticket>();
            int newTicketQuantity = 0;
            if (totalPoints / pointsForTicket >= 1)
            {
                var ticketInvestmentManager = iocResolver.Resolve<ITicketInvestmentManager>();

                decimal points = 0;
                for (int o = 0; o < recentOrders.Count; o++)
                {
                    var order = recentOrders[o];
                    var orderDetails = order.OrderDetails.ToArray();
                    for (int i = 0; i < orderDetails.Length; i++)
                    {
                        var detail = orderDetails[i];
                        if (detail.AvailablePoints > 0 && !string.IsNullOrEmpty(detail.SpoonCode))
                        {
                            points += detail.AvailablePoints;
                            if (points >= pointsForTicket)
                            {
                                decimal usedPoints = Math.Floor(points / pointsForTicket) * pointsForTicket;
                                int tickets = (int)Math.Floor(points / pointsForTicket);

                                points -= usedPoints;

                                for (int j = 0; j <= o; j++)
                                {
                                    var recentOrder = recentOrders[j];
                                    foreach (var recentDetail in recentOrder.OrderDetails)
                                    {
                                        if (recentDetail.AvailablePoints > 0)
                                        {
                                            decimal detailUsedPoints = usedPoints <= recentDetail.AvailablePoints
                                                ? usedPoints : recentDetail.AvailablePoints;

                                            await recentDetail.ApplyActionAsync(new OrderDetailUsePointAction(
                                                iocResolver,
                                                localizatonSource,
                                                detailUsedPoints)
                                            );

                                            usedPoints -= detailUsedPoints;

                                            if (Math.Round(usedPoints, 2) == 0) break;
                                        }
                                    }

                                    recentOrder.CalcTotal();

                                    if (Math.Round(usedPoints, 2) == 0) break;
                                }

                                for (int t = 1; t <= tickets; t++)
                                {
                                    issueTicketQuantity++;
                                    newTicketQuantity++;
                                    if (issueTicketQuantity <= totalTicketQuantity)
                                    {
                                        var ticket = await ticketInvestmentManager.GenerateTicketAsync(TicketInvestmentId.Value, ConsumerPhone, ConsumerName);
                                        actualNewTickets.Add(ticket);

                                        await order.ApplyActionAsync(new OrderAddTicketAction(iocResolver, localizatonSource, ticket.Id, ticket.Code, detail.QrCode));

                                    }
                                   
                                }
                            }
                        }
                    }
             
                }
            }
            return (actualNewTickets, newTicketQuantity);
        }

        private void CalcTotal()
        {
            TotalPoints = _orderDetails.Sum(p => p.Points);
            TotalAmount = _orderDetails.Sum(p => p.LineAmount);
            TotalQuantity = _orderDetails.Sum(p => p.Quantity);
            TotalAvailablePoints = _orderDetails.Sum(p => p.AvailablePoints);
            TotalUsedPoints = _orderDetails.Sum(p => p.UsedPoints);
        }

        private async Task SendSmsAsync(IIocResolver iocResolver, ILocalizationSource localizatonSource, Customer customer, List<Ticket> newTickets, int totalNewTickets)
        {
            var appSettingManager = iocResolver.Resolve<IAppSettingManager>();
            var smsMessageSender = iocResolver.Resolve<ISmsMessageSender>();
            var rewardAppLink = (await appSettingManager.GetAsync("REWARD_APP_LINK")) ?? "";
            string remainPoints = TotalAvailablePoints.ToString("N0");
            if (newTickets.Count > 0)
            {

                string ticketCodes = string.Join(",", newTickets.Select(p => p.Code).ToList());
                await smsMessageSender.ScheduleAsync(
                    ConsumerPhone,
                    localizatonSource.GetString(
                        await iocResolver.Resolve<IAppSettingManager>().GetAsync(TotalAvailablePoints > 0 ? "TICKET1_SMS_TEMPLATE" : "TICKET2_SMS_TEMPLATE"),
                        customer.Name,
                        newTickets.Count.ToString("N0"),
                        ticketCodes,
                        remainPoints,
                        rewardAppLink)
                    );
            }
            else
            {
                if (TotalAvailablePoints > 0)
                {
                    await smsMessageSender.ScheduleAsync(
                       ConsumerPhone,
                       localizatonSource.GetString(
                           await iocResolver.Resolve<IAppSettingManager>().GetAsync("TICKET3_SMS_TEMPLATE"),
                           customer.Name,
                           newTickets.Count.ToString("N0"),
                           "",
                           remainPoints,
                           rewardAppLink)
                    );
                }
               
            }
        }
    }
}