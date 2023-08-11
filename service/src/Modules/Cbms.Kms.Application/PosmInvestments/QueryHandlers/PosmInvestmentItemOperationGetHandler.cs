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
    public class PosmInvestmnetItemOperationGetHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentItemOperationGet, PosmInvestmentItemOperationDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PosmInvestmnetItemOperationGetHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<PosmInvestmentItemOperationDto> Handle(PosmInvestmentItemOperationGet request, CancellationToken cancellationToken)
        {
        
            string sql = @"
                SELECT 
	                *,
	                ConfirmUserName = op1.ConfirmUser
                FROM PosmInvestmentItems AS ptm
                OUTER APPLY (
	                SELECT ConfirmUser = (SELECt Name FROM Users AS u WHERE u.Id = JSON_VALUE(h.Data, '$.LastModifierUserId'))
	                FROM PosmInvestmentItemHistories AS h 
	                WHERE PosmInvestmentItemId = ptm.Id AND JSON_VALUE(DATA, '$.Status') = 160 
                ) AS op1
                WHERE ptm.Id = @InvestmentItemId
            ";
           
            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<PosmInvestmentItemOperationDto>(sql, new
            {
                InvestmentItemId = request.Id
            });
           
        }
    }
}