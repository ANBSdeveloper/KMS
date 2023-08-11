using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.CustomerSalesItems;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketInvestmentTrackingGetHandler : QueryHandlerBase, IRequestHandler<TicketInvestmentTrackingGet, TicketInvestmentTrackingDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly IRepository<CustomerSalesItem, int> _salesItemRepository;
        private readonly IRepository<OrderDetail, int> _orderDetailRepository;
        private readonly IRepository<Order, int> _orderRepository;
        private readonly ICustomerManager _customerManager;

        public TicketInvestmentTrackingGetHandler(
            IRequestSupplement supplement,
            IRepository<TicketInvestment, int> ticketInvestmentRepository,
            IRepository<CustomerSalesItem, int> salesItemRepository,
            IRepository<OrderDetail, int> orderDetailRepository,
            IRepository<Order, int> orderRepository,
        ICustomerManager customerManager) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _salesItemRepository = salesItemRepository;
            _customerManager = customerManager;
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
        }

        public async Task<TicketInvestmentTrackingDto> Handle(TicketInvestmentTrackingGet request, CancellationToken cancellationToken)
        {
            var ticketInvestment = await _ticketInvestmentRepository
                .GetAllIncluding(p => p.TicketAcceptance, prop => prop.Tickets)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (ticketInvestment == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), request.Id);
            }
            var salesQrCode = _salesItemRepository.GetAll()
                .Where(p => p.TicketInvestmentId == ticketInvestment.Id && !p.IsUsing)
                .Select(p => p.QrCode).ToList();

            var orderQrCode = (
                    from detail in _orderDetailRepository.GetAll()
                    join order in _orderRepository.GetAll() on detail.OrderId equals order.Id
                    where order.TicketInvestmentId == ticketInvestment.Id
                    select detail
                )
               .Select(p => p.QrCode)
               .ToList();
            salesQrCode.AddRange(orderQrCode);

            var qrcodeQuantity = salesQrCode.Distinct().Count();

            var tickets = from p in ticketInvestment.Tickets
                          select new TicketDto()
                          {
                              Code = p.Code,
                              ConsumerPhone = p.ConsumerPhone,
                              ConsumerName = p.ConsumerName,
                              Id = p.Id,
                              IssueDate = p.IssueDate,
                              LastPrintUserId = p.LastPrintUserId,
                              PrintCount = p.PrintCount,
                              PrintDate = p.PrintDate
                          };

            return new TicketInvestmentTrackingDto()
            {
                Id = ticketInvestment.Id,
                ConsumerQuantity = ticketInvestment.Tickets.Select(p => p.ConsumerPhone).Distinct().Count(),
                SentTicketQuantity = ticketInvestment.SmsTicketQuantity,
                QrCodeQuantity = qrcodeQuantity,
                BuyBeginDate = ticketInvestment.BuyBeginDate,
                BuyEndDate = ticketInvestment.BuyEndDate,
                IssueTicketBeginDate = ticketInvestment.IssueTicketBeginDate,
                IssueTicketEndDate = ticketInvestment.IssueTicketEndDate,
                OperationDate = ticketInvestment.OperationDate,
                Code = ticketInvestment.Code,
                Tickets = tickets.ToList()
            };
        }
    }
}