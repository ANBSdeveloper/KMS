using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments.Query;
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

namespace Cbms.Kms.Application.PosmInvestments.QueryHandler
{
    public class PosmInvestmentItemGetListByCustomerHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentItemGetListByCustomer, PagingResult<PosmInvestmentItemExtDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PosmInvestmentItemGetListByCustomerHandler(IRequestSupplement supplement, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PagingResult<PosmInvestmentItemExtDto>> Handle(PosmInvestmentItemGetListByCustomer request, CancellationToken cancellationToken)
        {
            string sql = $@"
                SELECT
                    Code = i.Code,
                    RegisterDate = i.CreationTime,
                    PosmInvestmentItemId = iv.Id,
                    PosmInvestmentId = i.Id,
                    PosmInvestmentCode = i.Code,
                    CustomerCode = c.Code,
                    CustomerId = c.Id,
                    ItemStatus = iv.Status,
                    Status = i.Status,
                    Email = c.Email,
                    CustomerName = c.Name,
                    MobilePhone = c.MobilePhone,
                    Address = c.Address,
                    RegisterStaffName = s.Name,
                    ZoneName = z.Name,
					AreaName = ar.Name,
                    PosmItemId = pi.Id,
                    PosmItemCode = pi.Code,
                    PosmItemName = pi.Name,
                    InvestmentAmount = IIF(iv.ActualTotalCost IS NOT NULL, iv.ActualTotalCost, iv.TotalCost),
                    ActualTotalCost = iv.ActualTotalCost,
                    TotalCost = iv.TotalCost,
                    RemarkOfSales = iv.RemarkOfSales,
                    RemarkOfCompany = iv.RemarkOfCompany,
                    UnitPrice = iv.ActualUnitPrice,
                    Qty = iv.Qty
                FROM PosmInvestments AS i
                INNER JOIN PosmInvestmentItems AS iv ON i.Id = iv.PosmInvestmentId
                INNER JOIN PosmItems AS pi ON iv.PosmItemId = pi.Id
                INNER JOIN Customers AS c ON c.Id = i.CustomerId
                INNER JOIN Zones AS z ON c.ZoneId = z.Id
                INNER JOIN Areas AS ar ON c.AreaId = ar.Id
                INNER JOIN Staffs AS s ON i.RegisterStaffId = s.Id
                WHERE i.CustomerId = ${request.CustomerId} "
                + (!string.IsNullOrEmpty(request.Keyword) ? @$"AND (
                            i.Code LIKE N'%{request.Keyword}%' OR
                            c.Name LIKE N'%{request.Keyword}%' OR
                            c.Code LIKE N'%{request.Keyword}%' OR
                            c.MobilePhone LIKE N'%{request.Keyword}%') " : "")
                + $@"ORDER BY i.CreationTime ";

            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var query = await connection.QueryAsync<PosmInvestmentItemExtDto>(sql);

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
            return new PagingResult<PosmInvestmentItemExtDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}