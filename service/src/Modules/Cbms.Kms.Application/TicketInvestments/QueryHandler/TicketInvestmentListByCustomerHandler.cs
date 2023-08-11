using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandler
{
    public class TicketInvestmentListByCustomerHandler : QueryHandlerBase, IRequestHandler<TicketInvestmnetGetListByCustomer, PagingResult<TicketInvestmentListItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public TicketInvestmentListByCustomerHandler(IRequestSupplement supplement, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<TicketInvestmentListItemDto>> Handle(TicketInvestmnetGetListByCustomer request, CancellationToken cancellationToken)
        {
            string sql = $@"
                SELECT
                    i.*,
                    RewardPackageName = r.Name,
                    CustomerCode = c.Code,
                    Email = c.Email,
                    CustomerName = c.Name,
                    MobilePhone = c.MobilePhone,
                    Address = c.Address,
                    RemarkOfCompany = a.RemarkOfCompany,
                    RegisterStaffName = s.Name,
                    ZoneName = z.Name,
					AreaName = ar.Name
                FROM TicketInvestments AS i
                LEFT JOIN TicketAcceptances AS a ON i.Id = a.TicketInvestmentId
                INNER JOIN Customers AS c ON c.Id = i.CustomerId
                INNER JOIN Zones AS z ON c.ZoneId = z.Id
                INNER JOIN Areas AS ar ON c.AreaId = ar.Id
                INNER JOIN RewardPackages AS r ON i.RewardPackageId = r.Id
                INNER JOIN Staffs AS s ON i.RegisterStaffId = s.Id
                WHERE i.CustomerId = ${request.CustomerId} "
                + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            i.Code LIKE N'%{request.Keyword}%' OR
                            c.Name LIKE N'%{request.Keyword}%' OR
                            c.Code LIKE N'%{request.Keyword}%' OR
                            c.MobilePhone LIKE N'%{request.Keyword}%') " : "")
                + $@"ORDER BY i.CreationTime ";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var query = await connection.QueryAsync<TicketInvestmentListItemDto>(sql);

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<TicketInvestmentListItemDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}