using Cbms.Kms.Application.Orders.Dto;
using Cbms.Kms.Application.Orders.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.QueryHandler
{
    public class OrderGetListForShopHandler : QueryHandlerBase, IRequestHandler<OrderGetListForShop, List<OrderListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OrderGetListForShopHandler(IRequestSupplement supplement, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<List<OrderListItemDto>> Handle(OrderGetListForShop request, CancellationToken cancellationToken)
        {
            string sql = $@"
                        SELECT
                            i.*,
                            CustomerName = c.Name,
                            CustomerCode = c.Code,
                            TicketInvestmentCode = t.Code,
                            TicketCodes =  STUFF(
                                (   SELECT ', ' + ticket.Code 
                                    FROM OrderTickets AS orderTicket
                                    JOIN Tickets AS ticket ON orderTicket.TicketId = ticket.Id
                                    WHERE orderTicket.OrderId = i.Id
                                    FOR XML PATH ('')
                                ), 1, 2, ''
                            )
                        FROM Orders AS i
                        INNER JOIN Customers AS c ON i.CustomerId = c.Id
                        INNER JOIN TicketInvestments AS t ON i.TicketInvestmentId = t.Id
                        WHERE c.UserId = {Session.UserId.Value} AND i.TicketInvestmentId = {request.TicketInvestmentId}";

            using (var connection = await _sqlConnectionFactory.GetConnectionAsync())
            {
                var items = await connection.QueryAsync<OrderListItemDto>(sql);

                return items.ToList();
            }
               
         
            
        }
    }
}