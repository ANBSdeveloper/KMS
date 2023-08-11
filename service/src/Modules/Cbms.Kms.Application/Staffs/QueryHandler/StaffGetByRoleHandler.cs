using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Kms.Application.Staffs.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using Cbms.Mediator.Query;
using Cbms.Mediator.Query.Pagination;
using Cbms.Runtime.Connection;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Staffs.QueryHandler
{
    public class StaffGetByRoleHandler : QueryHandlerBase, IRequestHandler<StaffGetByRole, StaffListDto>
    {

        private readonly AppDbContext _dbContext;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;


        public StaffGetByRoleHandler(IRequestSupplement supplement, AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory) : base(supplement)
        {
            _dbContext = dbContext;
            _sqlConnectionFactory = sqlConnectionFactory;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }        

        public async Task<StaffListDto> Handle(StaffGetByRole request, CancellationToken cancellationToken)
        {

            if (request.SupervisorId.HasValue)
            {
                var supervisorStaff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.Id == request.SupervisorId);
                if (supervisorStaff == null)
                {
                    throw new EntityNotFoundException(typeof(Staff), request.SupervisorId);
                }
                else
                {
                    return await GetItemsFromSalesOrgAsync(request, supervisorStaff.SalesOrgId);
                }
            }
            else
            {
                var staff = await _dbContext.Staffs.FirstOrDefaultAsync(p => p.UserId == Session.UserId);
                if (staff != null)
                {
                    return await GetItemsFromSalesOrgAsync(request, staff.SalesOrgId);
                }

                return await GetItemsFromUserAsync(request);
            }
        }

        private async Task<StaffListDto> GetItemsFromSalesOrgAsync(StaffGetByRole request, int salesOrgId)
        {
            string sql = $@"
                                WITH CTE AS
                                (
                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
        	                        WHERE Id = {salesOrgId}

                                    UNION ALL

                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                                )
                                SELECT s.Code, s.Name, s.StaffTypeCode, s.StaffTypeName, s.UpdateDate, s.MobilePhone, s.Birthday, s.Email 
                                FROM Staffs AS s
                                WHERE s.StaffTypeCode = '{request.StaffTypeCode}' AND s.Id = '{request.Id}' 
                                     AND s.SalesOrgId IN (
                                    SELECT CTE.Id FROM CTE
                                ) ";



            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<StaffListDto>(sql);


            return items.FirstOrDefault();
        }


        private async Task<StaffListDto> GetItemsFromUserAsync(StaffGetByRole request)
        {
            string sql = $@"

                                WITH CTE AS
                                (
                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
        	                        WHERE UserAssignments.UserId = {Session.UserId}

                                    UNION ALL

                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
                                )
                                SELECT s.Code, s.Name, s.StaffTypeCode, s.StaffTypeName, s.UpdateDate, s.MobilePhone, s.Birthday, s.Email
                                FROM Staffs AS s
                                WHERE s.StaffTypeCode = '{request.StaffTypeCode}' AND s.Id = '{request.Id}'  
                                AND s.SalesOrgId IN (
                                    SELECT CTE.Id FROM CTE
                                )";


            var connection = await _sqlConnectionFactory.GetConnectionAsync();
            var items = await connection.QueryAsync<StaffListDto>(sql);


            return items.FirstOrDefault();
        }

    }

}



