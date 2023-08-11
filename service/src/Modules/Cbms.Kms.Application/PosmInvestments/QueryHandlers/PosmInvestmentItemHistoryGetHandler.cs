using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.QueryHandler
{
    public class PosmInvestmentItemHistoryGetHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentItemHistoryGet, PosmInvestmentItemHistoryDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PosmInvestmentItemHistoryGetHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PosmInvestmentItemHistoryDto> Handle(PosmInvestmentItemHistoryGet request, CancellationToken cancellationToken)
        {
            //string sql = @"
            //    SELECT 
		          //  h.*,
            //        Status = JSON_VALUE(h.Data, '$.Status'),
		          //  CreationRole = (
			         //   SELECT Top 1 r.RoleName 
			         //   FROM UserRoles AS ur
			         //   INNER JOIN Roles AS r ON ur.RoleId = r.Id
			         //   WHERE ur.UserId  = JSON_VALUE(h.Data, '$.LastModifierUserId')
		          //  ),
		          //  CreationUser = (SELECT Name FROM Users AS u WHERE u.Id = JSON_VALUE(h.Data, '$.LastModifierUserId')),
            //        RemarkOfSales,
            //        RemarkOfCompany
	           // FROM PosmInvestmentItemHistories AS h 
            //    INNER JOIN PosmInvestmentItems AS i ON h.PosmInvestmentItemId = i.Id
	           // WHERE h.PosmInvestmentItemId = @PosmInvestmentItemId
            //    ORDER BY CreationTime
            //";
            string sql = @"
                SELECT 
		            h.*,
                    Status = JSON_VALUE(h.Data, '$.Status'),
		            CreationRole = (
			            SELECT Top 1 r.RoleName 
			            FROM UserRoles AS ur
			            INNER JOIN Roles AS r ON ur.RoleId = r.Id
			            WHERE ur.UserId  = h.CreatorUserId
		            ),
		            CreationUser = (SELECT Name FROM Users AS u WHERE u.Id = h.CreatorUserId),
                    RemarkOfSales,
                    RemarkOfCompany
	            FROM PosmInvestmentItemHistories AS h 
                INNER JOIN PosmInvestmentItems AS i ON h.PosmInvestmentItemId = i.Id
	            WHERE h.PosmInvestmentItemId = @PosmInvestmentItemId
                ORDER BY CreationTime
            ";
            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<PosmInvestmnetItemHistoryItemDto>(sql, new { PosmInvestmentItemId = request.Id});

            var result = new PosmInvestmentItemHistoryDto();
        
            foreach (var item in items)
            {
                if (item.Status == Domain.PosmInvestments.PosmInvestmentItemStatus.Request) {
                    result.RequestItems.Add(item);
                } 
                else if (item.Status > Domain.PosmInvestments.PosmInvestmentItemStatus.Request && item.Status <= Domain.PosmInvestments.PosmInvestmentItemStatus.DirectorApprovedRequest)
                {
                    result.ApproveItems.Add(item);
                }
                else if (item.Status > Domain.PosmInvestments.PosmInvestmentItemStatus.DirectorApprovedRequest && item.Status <= Domain.PosmInvestments.PosmInvestmentItemStatus.ValidOrder)
                {
                    result.PrepareItems.Add(item);
                }
                else if (item.Status > Domain.PosmInvestments.PosmInvestmentItemStatus.ValidOrder && item.Status <= Domain.PosmInvestments.PosmInvestmentItemStatus.ConfirmedVendorProduce)
                {
                    result.OperationItems.Add(item);
                }
                else if (item.Status > Domain.PosmInvestments.PosmInvestmentItemStatus.ConfirmedVendorProduce && item.Status <= Domain.PosmInvestments.PosmInvestmentItemStatus.ConfirmedAccept2)
                {
                    result.AcceptanceItems.Add(item);
                }
            }

            return result;
            
        }
    }
}