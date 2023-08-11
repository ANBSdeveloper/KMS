using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.QueryHandler
{
    public class OrderDetailGetListByOrderIdHandler : QueryHandlerBase, IRequestHandler<OrderDetailGetListByOrderId, PagingResult<OrderDetailDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public OrderDetailGetListByOrderIdHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagingResult<OrderDetailDto>> Handle(OrderDetailGetListByOrderId request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            string query = $@"select 
                            a.ProductCode
                            , a.ProductId
                            , a.ProductName
                            , a.QrCode
                            , a.SpoonCode
                            , a.UnitName
                            , a.Quantity
                            , a.UnitPrice
                            , a.LineAmount
                            , a.Api
                            , a.Points
                            , a.AvailablePoints
                            , a.UsedPoints
                            , a.UsedForTicket
                            , a.LastModificationTime
                            from OrderDetails as a
                            where a.OrderId = {request.OrderId} ";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<OrderDetailDto>(query);
            items = items.ToList();

            int totalCount = items.Count();           
            return new PagingResult<OrderDetailDto>()
            {
                Items = items.ToList(),
                TotalCount = totalCount
            };
        }

    }
}
