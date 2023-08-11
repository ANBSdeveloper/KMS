using Cbms.Domain.Entities;
using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.QueryHandlers
{
    public class OrderGetHandler : QueryHandlerBase, IRequestHandler<OrderGet, OrderDto>
    {
        private readonly AppDbContext _dbContext;

        public OrderGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<OrderDto> Handle(OrderGet request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Orders
                .Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Order), request.Id);
            }

            var entityDto = Mapper.Map<OrderDto>(entity);

            entityDto.OrderDetails = await (from p in _dbContext.OrderDetails
                                            where p.OrderId == entity.Id
                                            select new OrderDetailDto()
                                            {
                                                Api = p.Api,
                                                AvailablePoints = p.AvailablePoints,
                                                CreationTime = p.CreationTime,
                                                CreatorUserId = p.CreatorUserId,
                                                Id = p.Id,
                                                LastModificationTime = p.LastModificationTime,
                                                LastModifierUserId = p.LastModifierUserId,
                                                LineAmount = p.LineAmount,
                                                Points = p.Points,
                                                ProductCode = p.ProductCode,
                                                ProductId = p.ProductId,
                                                ProductName = p.ProductName,
                                                QrCode = p.QrCode,
                                                Quantity = p.Quantity,
                                                SpoonCode = p.SpoonCode,
                                                UnitName = p.UnitName,
                                                UnitPrice = p.UnitPrice,
                                                UsedForTicket = p.UsedForTicket,
                                                UsedPoints = p.UsedPoints,
                                            }).ToListAsync();

            entityDto.Tickets = await (from p in _dbContext.OrderTickets
                                       join i in _dbContext.Tickets on p.TicketId equals i.Id
                                       where p.OrderId == entity.Id
                                       select new OrderTicketDto()
                                       {
                                           CreationTime = p.CreationTime,
                                           CreatorUserId = p.CreatorUserId,
                                           Id = p.Id,
                                           LastModificationTime = p.LastModificationTime,
                                           LastModifierUserId = p.LastModifierUserId,
                                           TicketCode = i.Code,
                                           TicketId = p.TicketId
                                       }).ToListAsync();
            return entityDto;
        }
    }
}