using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerGetRecentSalesHandler : QueryHandlerBase, IRequestHandler<CustomerGetRecentSales, CustomerRecentSalesDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IAppSettingManager _appSettingManager;
        public CustomerGetRecentSalesHandler(
            IRequestSupplement supplement, 
            AppDbContext dbContext, 
            ISqlConnectionFactory sqlConnectionFactory,
            IAppSettingManager appSettingManager) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            _appSettingManager = appSettingManager;
        }

        public async Task<CustomerRecentSalesDto> Handle(CustomerGetRecentSales request, CancellationToken cancellationToken)
        {
          
            string tempMonthTableSql = $@"
                    DECLARE @Months AS TABLE (Year [int], Month [int])
                    INSERT INTO @Months([Year],[Month]) VALUES ";

            var monthSettingValue = await _appSettingManager.GetAsync("DMS_SELLOUT_MONTHS");

            int months;
            if (string.IsNullOrEmpty(monthSettingValue) || !int.TryParse(monthSettingValue, out months))
            {
                months = 6;
            }

            if (months <= 0)
            {
                months = 0;
            }
            else if (months > 12)
            {
                months = 12;
            }

            for (int i = 0; i <= months; i++)
            {
                var date = request.Date.AddMonths(i * -1);
                int year = date.Year;
                int month = date.Month;

                tempMonthTableSql += $"({year},{month}),";
            }
            tempMonthTableSql = tempMonthTableSql.TrimEnd(',');
            string monthSql = $@"
                {tempMonthTableSql}
                SELECT 
	                [Year] = CAST(m.[Year] as int),
	                [Month] = CAST(m.[Month] as int), 
	                Amount = ISNULL(Amount, 0)
                FROM @Months AS m
                LEFT JOIN  CustomerSales AS s ON m.[Year] = s.[Year] AND m.[Month] = s.[Month] AND CustomerId = {request.Id} 
                ORDER BY m.[Year], m.[Month]
                ";

            string yearSql = $@"
                DECLARE @Years AS TABLE (Year [int])
                INSERT INTO @Years([Year]) 
                VALUES ({request.Date.Year}), ({request.Date.Year - 1})

                SELECT 
	                Measure = 'FY ' + CAST(y.[Year] AS VARCHAR),
	                Amount = ISNULL(fy.Amount, 0)
                FROM @Years as y
                OUTER APPLY (
	                SELECT Amount = SUM(Amount) 
	                FROM CustomerSales
	                WHERE CustomerId = {request.Id} and [Year] = y.[Year]
	                GROUP BY [Year]
                ) AS fy
                UNION ALL 

                SELECT 
	                Measure = 'YTD ' + + CAST(y.[Year] AS VARCHAR),
	                Amount = ISNULL(tyd.Amount, 0)
                FROM @Years as y
                OUTER APPLY (
	                SELECT Amount = SUM(Amount) 
	                FROM CustomerSales
	                WHERE CustomerId = {request.Id} and [Year] = y.[Year] AND [Month] <= {request.Date.Month}
	                GROUP BY [Year]
                ) AS tyd";


            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var monthItems = await connection.QueryAsync<MonthData>(monthSql);
            var yearItems = await connection.QueryAsync<YearData>(yearSql);

            return new CustomerRecentSalesDto()
            {
                MonthData = monthItems.ToList(),
                YearData = yearItems.ToList()
            };
        }
    }
}
