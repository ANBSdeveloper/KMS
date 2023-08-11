using Cbms.Authentication;
using Cbms.Authorization.Roles;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Helpers;
using Cbms.Kms.Application.Integration.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Areas.Actions;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Branches.Actions;
using Cbms.Kms.Domain.Brands;
using Cbms.Kms.Domain.Brands.Actions;
using Cbms.Kms.Domain.Channels;
using Cbms.Kms.Domain.Channels.Actions;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Kms.Domain.CustomerSales;
using Cbms.Kms.Domain.CustomerSales.Actions;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Geography.Districts;
using Cbms.Kms.Domain.Geography.Districts.Actions;
using Cbms.Kms.Domain.Geography.Provinces;
using Cbms.Kms.Domain.Geography.Provinces.Actions;
using Cbms.Kms.Domain.Geography.Wards;
using Cbms.Kms.Domain.Geography.Wards.Actions;
using Cbms.Kms.Domain.OracleProvider;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Kms.Domain.ProductClasses.Actions;
using Cbms.Kms.Domain.ProductPrices;
using Cbms.Kms.Domain.ProductPrices.Actions;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.Products.Actions;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Kms.Domain.ProductUnits.Actions;
using Cbms.Kms.Domain.SalesOrgs;
using Cbms.Kms.Domain.SalesOrgs.Actions;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Staffs.Actions;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Kms.Domain.SubProductClasses.Actions;
using Cbms.Kms.Domain.Users;
using Cbms.Kms.Domain.UserSalesOrgs.Actions;
using Cbms.Kms.Domain.Zones;
using Cbms.Kms.Domain.Zones.Actions;
using Cbms.Kms.Infrastructure;
using Cbms.Kms.Infrastructure.PgSqlProvider;
using Dapper;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Integration
{
    public class SyncDataJob
    {
        private readonly IAppLogger _appLogger;
        private readonly AppDbContext _dbContext;
        private readonly IIocResolver _iocResolver;
        private readonly ILogger _logger;
        private readonly IOracleDataAccess _oracleDataAccess;
        private readonly IPgConnectionFactory _pgConnectionFactory;
        public SyncDataJob(
            IIocResolver iocResolver,
            ILogger logger,
            IOracleDataAccess oracleDataAccess,
            IPgConnectionFactory pgConnectionFactory,
            IAppLogger appLogger,
            AppDbContext dbContext)
        {
            _appLogger = appLogger;
            _iocResolver = iocResolver;
            _logger = logger;
            _oracleDataAccess = oracleDataAccess;
            _pgConnectionFactory = pgConnectionFactory;
            _dbContext = dbContext;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task RunAsync()
        {
            try
            {
                await SyncProductAsync();
                await SyncSalesOrgAsync();
                await SyncZoneAsync();
                await SyncAreaAsync();
                await SyncProvinceAsync();
                await SyncDistrictSync();
                await SyncWardAsync();
                await SyncChannelAsync();
                await SyncBranchAsync();
                await SyncStaffAsync();
                await SyncCustomerAsync();
                await SyncCustomerMapAsync();
                await SyncCustomerSalesAsync();
                await SyncProductUnitAsync();
                await SyncProductPriceAsync();
                await SyncPgUserAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Integration");
                throw ex;
            }
        }

        private async Task SyncAreaAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_AREA", new { Message = "Start" });
            try
            {
                var areaRepository = _iocResolver.Resolve<IRepository<Area, int>>();
                var zoneRepository = _iocResolver.Resolve<IRepository<Zone, int>>();
                string sql = @"
                        SELECT
                            a.SHOP_CODE AS Code,
                            a.SHOP_NAME AS Name,
                            z.SHOP_CODE AS ZoneId,
                            a.SHOP_ID AS SalesOrgId
                        FROM VITADAIRY.SHOP a
                        INNER JOIN VITADAIRY.SHOP z ON a.PARENT_SHOP_ID = z.SHOP_ID
                        WHERE a.SHOP_TYPE_ID = 1145";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<AreaDto> lstArea = DataTableHelper.DataTableToList<AreaDto>(table);

                var distinctArea = lstArea.GroupBy(p => p.Code).Select(p => new { Code = p.Key, Name = p.FirstOrDefault().Name, ZoneId = p.FirstOrDefault().ZoneId, SalesOrgId = p.FirstOrDefault().SalesOrgId }).ToList();

                foreach (var item in distinctArea)
                {
                    var zone = await zoneRepository.FirstOrDefaultAsync(p => p.Code == item.ZoneId);
                    if (zone != null)
                    {
                        var area = await areaRepository.GetAll().FirstOrDefaultAsync(p => p.Code == item.Code);
                        if (area == null)
                        {
                            area = Area.Create();
                            await areaRepository.InsertAsync(area);
                        }

                        await area.ApplyActionAsync(new UpsertAreaAction(
                            item.Code,
                            item.Name,
                            zone.Id,
                            item.SalesOrgId

                        ));
                    }
                }
                await areaRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_AREA", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_AREA", ex);
                throw ex;
            }
        }

        private async Task SyncBranchAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_BRANCH", new { Message = "Start" });
            try
            {
                var branchRepository = _iocResolver.Resolve<IRepository<Branch, int>>();
                var areaRepository = _iocResolver.Resolve<IRepository<Area, int>>();
                var channelRepository = _iocResolver.Resolve<IRepository<Channel, int>>();
                DateTime updateTime = new DateTime(2000, 1, 1);
                var branchupdateMax = await branchRepository.Query(p => p.OrderByDescending(x => x.UpdateDate)).FirstOrDefaultAsync();
                if (branchupdateMax != null)
                {
                    updateTime = branchupdateMax.UpdateDate == null ? updateTime : DateTime.Parse(branchupdateMax.UpdateDate.ToString());
                }

                string sql = $@"SELECT
                                SHOP_CODE AS Code,
                                SHOP_NAME AS Name,
                                STATUS AS IsActive,
                                SHOP_ID AS SalesOrgId,
                                NVL(UPDATE_DATE, CREATE_DATE) AS UpdateDate
                            FROM VITADAIRY.SHOP s
                            WHERE SHOP_TYPE_ID = 1146
                            AND NVL(UPDATE_DATE, CREATE_DATE) >=to_date('{updateTime.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                            ORDER BY NVL(UPDATE_DATE, CREATE_DATE) ASC";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<Branch> branches = DataTableHelper.DataTableToList<Branch>(table);

                foreach (var item in branches)
                {
                    var branch = await branchRepository.FirstOrDefaultAsync(p => p.Code == item.Code);
                    if (branch == null)
                    {
                        branch = Branch.Create();
                        await branchRepository.InsertAsync(branch);
                    }

                    var areaOrg = _dbContext.SalesOrgs.FromSqlRaw(@$"
                            WITH CTE AS
                            (
	                            SELECT SalesOrgs.*
                                FROM   SalesOrgs
                                WHERE  SalesOrgs.Id = {item.SalesOrgId}
                                UNION ALL

                                SELECT SalesOrgs.*
                                FROM   SalesOrgs
                                INNER JOIN CTE     ON SalesOrgs.id = CTE.ParentId
                            )
                            SELECT *
                            FROM   CTE
                            WHERE [TypeId] = 1145
                        ").ToList().FirstOrDefault();

                    var channelOrg = _dbContext.SalesOrgs.FromSqlRaw(@$"
                            WITH CTE AS
                            (
	                            SELECT SalesOrgs.*
                                FROM   SalesOrgs
                                WHERE  SalesOrgs.Id = {item.SalesOrgId}
                                UNION ALL

                                SELECT SalesOrgs.*
                                FROM   SalesOrgs
                                INNER JOIN CTE     ON SalesOrgs.id = CTE.ParentId
                            )
                            SELECT *
                            FROM   CTE
                            WHERE [TypeId] = 2
                        ").ToList().FirstOrDefault();

                    int? areaId = null;
                    int? zoneId = null;
                    int? channelId = null;

                    if (areaOrg != null)
                    {
                        var area = await areaRepository.FirstOrDefaultAsync(p => p.SalesOrgId == areaOrg.Id);
                        if (area != null)
                        {
                            areaId = area.Id;
                            zoneId = area.ZoneId;
                        }
                    }

                    if (channelOrg != null)
                    {
                        var channel = await channelRepository.FirstOrDefaultAsync(p => p.SalesOrgId == channelOrg.Id);
                        if (channel != null)
                        {
                            channelId = channel.Id;
                        }
                    }

                    await branch.ApplyActionAsync(new UpsertBranchAction(
                        item.Code,
                        item.Name,
                        item.IsActive,
                        item.SalesOrgId,
                        item.UpdateDate,
                        areaId,
                        zoneId,
                        channelId
                    ));
                }
                await branchRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_BRANCH", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_BRANCH", ex);
                throw ex;
            }
        }

        private async Task SyncChannelAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_CHANNEL", new { Message = "Start" });
            try
            {
                var channelRepository = _iocResolver.Resolve<IRepository<Channel, int>>();
                string sql = @"
                    SELECT
                     SHOP_ID AS SalesOrgId,
                        SHOP_CODE AS Code,
                        SHOP_NAME AS Name
                    FROM VITADAIRY.SHOP
                    WHERE SHOP_TYPE_ID = 2";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<Channel> lstChannel = DataTableHelper.DataTableToList<Channel>(table);
                var distictChannels = lstChannel.GroupBy(p => p.Code).Select(p => new { Code = p.Key, p.FirstOrDefault().Name, p.FirstOrDefault().SalesOrgId }).ToList();

                foreach (var item in distictChannels)
                {
                    var channel = await channelRepository.GetAll().FirstOrDefaultAsync(p => p.Code == item.Code);
                    if (channel == null)
                    {
                        channel = Channel.Create();
                        await channelRepository.InsertAsync(channel);
                    }

                    await channel.ApplyActionAsync(new UpsertChannelsAction(
                        item.Code,
                        item.Name,
                        item.SalesOrgId
                    ));
                }
                await channelRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_CHANNEL", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_CHANNEL", ex);
                throw ex;
            }
        }

        private async Task SyncCustomerAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_CUSTOMER", new { Message = "Start" });
            try
            {
                var customerRepository = _iocResolver.Resolve<IRepository<Customer, int>>();
                var branchRepository = _iocResolver.Resolve<IRepository<Branch, int>>();
                var provinceRepository = _iocResolver.Resolve<IRepository<Province, int>>();
                var districtRepository = _iocResolver.Resolve<IRepository<District, int>>();
                var wardRepository = _iocResolver.Resolve<IRepository<Ward, int>>();
                var zoneRepository = _iocResolver.Resolve<IRepository<Zone, int>>();
                var areaRepository = _iocResolver.Resolve<IRepository<Area, int>>();
                var staffRepository = _iocResolver.Resolve<IRepository<Staff, int>>();

                var startDate = new DateTime(2000, 1, 1);
                var maxDate = await customerRepository.Query(p => p.OrderByDescending(x => x.UpdateDate)).FirstOrDefaultAsync();
                if (maxDate != null)
                {
                    startDate = maxDate.UpdateDate.Value;
                }

                string sql = $@"SELECT
                c.SHOP_ID AS SalesOrgId,
                c.SHORT_CODE AS Code,
                c.CUSTOMER_NAME AS Name,
                c.CONTACT_NAME AS ContactName,
                c.MOBIPHONE AS MobiPhone,
                c.PHONE AS Phone,
                c.EMAIL AS Email,
                ct.CHANNEL_TYPE_CODE AS ChannelTypeCode,
                ct.CHANNEL_TYPE_NAME AS ChannelTypeName,
                c.HOUSENUMBER AS HouseNumber,
                c.STREET AS Street,
                c.ADDRESS AS Address,
                a.PRECINCT AS WardCode,
                a.DISTRICT AS DistrictCode,
                a.PROVINCE AS ProvinceCode,
                c.STATUS AS IsActive,
                c.LAT AS Lat,
                c.LNG AS Lng,
                c.BIRTHDAY AS Birthday,
                NVL(c.UPDATE_DATE, c.CREATE_DATE) AS UpdateDate,
                c.STATUS AS IsActive
                FROM VITADAIRY.CUSTOMER c
                INNER JOIN VITADAIRY.CHANNEL_TYPE ct ON c.CHANNEL_TYPE_ID = ct.CHANNEL_TYPE_ID
                INNER JOIN VITADAIRY.AREA a ON c.AREA_ID = a.AREA_ID
                WHERE c.Status IN (0,1) AND NVL(c.UPDATE_DATE, c.CREATE_DATE) > to_date('{startDate.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                ORDER BY NVL(c.UPDATE_DATE, c.CREATE_DATE) ASC";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<CustomerDto> customers = DataTableHelper.DataTableToList<CustomerDto>(table);
                int count = 0;

                foreach (var item in customers)
                {
                    count++;

                    int? branchId = null;
                    int? districtId = null;
                    int? provinceId = null;
                    int? wardId = null;
                    int? rsmStaffId = null;
                    int? asmStaffId = null;
                    int? salesSupervisorStaffId = null;

                    var objBranch = await branchRepository.FirstOrDefaultAsync(p => p.SalesOrgId == item.SalesOrgId);
                    if (objBranch != null)
                    {
                        branchId = objBranch.Id;
                        var salesSupervisorStaff = await staffRepository.FirstOrDefaultAsync(p => p.SalesOrgId == objBranch.SalesOrgId && p.StaffTypeCode == "SS" && p.IsActive == true);
                        if (salesSupervisorStaff != null)
                        {
                            salesSupervisorStaffId = salesSupervisorStaff.Id;
                        }
                    }

                    var objDistrict = await districtRepository.FirstOrDefaultAsync(p => p.Code == item.DistrictCode);
                    if (objDistrict != null)
                    {
                        districtId = objDistrict.Id;
                    }

                    var objProvince = await provinceRepository.FirstOrDefaultAsync(p => p.Code == item.ProvinceCode);
                    if (objProvince != null)
                    {
                        provinceId = objProvince.Id;
                    }

                    var objWard = await wardRepository.FirstOrDefaultAsync(p => p.Code == item.WardCode);
                    if (objWard != null)
                    {
                        wardId = objWard.Id;
                    }

                    int? areaId = null;
                    int? zoneId = null;

                    var areaOrg = _dbContext.SalesOrgs.FromSqlRaw(@$"
                        WITH CTE AS
                        (
	                        SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            WHERE  SalesOrgs.Id = {item.SalesOrgId}
                            UNION ALL

                            SELECT SalesOrgs.*
                            FROM   SalesOrgs
                            INNER JOIN CTE     ON SalesOrgs.id = CTE.ParentId
                        )
                        SELECT *
                        FROM   CTE
                        WHERE [TypeId] = 1145
                    ").ToList().FirstOrDefault();

                    if (areaOrg != null)
                    {
                        var area = await areaRepository.FirstOrDefaultAsync(p => p.SalesOrgId == areaOrg.Id);
                        if (area != null)
                        {
                            areaId = area.Id;
                            zoneId = area.ZoneId;

                            var asmStaff = await staffRepository.FirstOrDefaultAsync(p => p.SalesOrgId == area.SalesOrgId && p.StaffTypeCode == "ASM" && p.IsActive == true);
                            if (asmStaff != null)
                            {
                                asmStaffId = asmStaff.Id;
                            }

                            var zone = await zoneRepository.FirstOrDefaultAsync(p => p.Id == zoneId);
                            var rsmStaff = await staffRepository.FirstOrDefaultAsync(p => p.SalesOrgId == zone.SalesOrgId && p.StaffTypeCode == "RSM" && p.IsActive == true);
                            if (rsmStaff != null)
                            {
                                rsmStaffId = rsmStaff.Id;
                            }
                        }
                    }

                    var customer = await customerRepository.FirstOrDefaultAsync(p => p.Code == item.Code);
                    bool isNew = false;

                    if (customer == null)
                    {
                        isNew = true;
                        customer = Customer.Create();
                        await customerRepository.InsertAsync(customer);
                    }

                    await customer.ApplyActionAsync(new CustomerUpsertAction(
                        isNew,
                        item.Code,
                        item.Name,
                        branchId,
                        item.ContactName,
                        item.MobiPhone ?? "",
                        item.Phone ?? "",
                        item.Email,
                        item.ChannelTypeCode,
                        item.ChannelTypeName,
                        item.HouseNumber,
                        item.Street,
                        item.Address,
                        wardId,
                        districtId,
                        provinceId,
                        item.IsActive,
                        item.Lat,
                        item.Lng,
                        item.Birthday,
                        item.UpdateDate,
                        areaId,
                        zoneId,
                        rsmStaffId,
                        asmStaffId,
                        salesSupervisorStaffId

                    ));
                    if (count == 100)
                    {
                        count = 0;
                        await customerRepository.UnitOfWork.CommitAsync();
                    }
                }
                await customerRepository.UnitOfWork.CommitAsync();

                await _appLogger.LogInfoAsync("SYNC_CUSTOMER", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_CUSTOMER", ex);
                throw ex;
            }
        }

        private async Task SyncCustomerMapAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_CUSTOMER_MAP", new { Message = "Start" });
            try
            {
                string sql = $@"
                 WITH
                    lstGS(user_id, staff_id, shop_id) AS (
	                    SELECT
		                    mp.user_id,
		                    mp.staff_id,
		                    mp.shop_id
	                    FROM
		                    VITADAIRY.MAP_USER_STAFF mp
	                    WHERE mp.specific_type = 1
	                    AND mp.user_specific_type = 2
	                    AND mp.from_date < trunc(sysdate) + 1
	                    AND (TRUNC (mp.TO_DATE) >= trunc(sysdate) OR mp.TO_DATE IS NULL)
	                    AND mp.staff_id IN (
			                    SELECT staff_id
			                    FROM VITADAIRY.staff
		                    )
                    ),
                    lstSaleRoute(staff_id, shop_id, routing_id) AS (
	                    SELECT
		                    vp.staff_id,
		                    vp.shop_id,
		                    vp.routing_id
	                    FROM
		                    VITADAIRY.visit_plan vp
	                    WHERE vp.from_date < trunc(sysdate) + 1
	                    AND (TRUNC (vp.TO_DATE) >= trunc(sysdate) OR vp.TO_DATE IS NULL)
	                    AND vp.status = 1
                    )
                    SELECT DISTINCT
                        sh.shop_code BranchCode,
	                    gs.staff_code SsStaffCode,
	                    st.staff_code SrStaffCode,
	                    c.short_code CustomerCode
                    FROM
	                    VITADAIRY.staff st
                    JOIN lstGS mp ON
	                    mp.staff_id = st.staff_id
                    JOIN VITADAIRY.staff gs ON
	                    gs.staff_id = mp.user_id
	                    AND gs.status = 1
                    JOIN VITADAIRY.shop sh ON
	                    mp.shop_id = sh.shop_id
                    JOIN lstSaleRoute sr ON
	                    mp.staff_id = sr.staff_id
	                    AND mp.shop_id = sr.shop_id
                    JOIN VITADAIRY.routing r ON
	                    sr.routing_id = r.routing_id
                    JOIN VITADAIRY.routing_customer rc ON
	                    sr.routing_id = rc.routing_id
	                    AND rc.start_date < trunc(sysdate) + 1
	                    AND (TRUNC (rc.end_date) >= trunc(sysdate)
		                    OR rc.end_date IS NULL)
                    JOIN VITADAIRY.customer c ON
	                    rc.customer_id = c.customer_id
                    WHERE c.SHORT_CODE IS NOT NULL

                ";

                DataTable sourceTable = _oracleDataAccess.ExecuteQuery(sql);

                var configuration = _iocResolver.Resolve<IConfiguration>();

                using (var sqlConnection = new SqlConnection(configuration["ConnectionString"]))
                {
                    await sqlConnection.OpenAsync();
                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        SqlCommand command = new SqlCommand("TRUNCATE TABLE CustomerMap", sqlConnection, transaction);
                        command.CommandTimeout = 3600;
                        await command.ExecuteNonQueryAsync();
                        using (SqlBulkCopy bcopy = new SqlBulkCopy(
                               sqlConnection, SqlBulkCopyOptions.KeepIdentity,
                               transaction))
                        {
                            try
                            {
                                bcopy.DestinationTableName = "CustomerMap";

                                bcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BranchCode", "BranchCode"));
                                bcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SsStaffCode", "SsStaffCode"));
                                bcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SrStaffCode", "SrStaffCode"));
                                bcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CustomerCode", "CustomerCode"));

                                bcopy.WriteToServer(sourceTable);

                                SqlCommand updateCommand = new SqlCommand(@$"UPDATE customer SET
                                    customer.BranchId = branch.Id,
                                    customer.SalesSupervisorStaffId = staff.Id
                                    FROM
                                    Customers AS customer
                                    LEFT JOIN CustomerMap AS map ON customer.Code = map.CustomerCode
                                    LEFT JOIN Staffs AS staff ON map.SSStaffCode = staff.Code
                                    LEFT JOIN Branches AS branch on map.BranchCode = branch.Code", sqlConnection, transaction);
                                updateCommand.CommandTimeout = 3600;
                                await updateCommand.ExecuteNonQueryAsync();

                                await transaction.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                throw ex;
                            }
                        }
                   
                    }
                }

                await _appLogger.LogInfoAsync("SYNC_CUSTOMER_MAP", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_CUSTOMER_MAP", ex);
                throw ex;
            }
        }

        private async Task SyncCustomerSalesAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_CUSTOMER_SALES", new { Message = "Start" });
            try
            {
                var customerRepository = _iocResolver.Resolve<IRepository<Customer, int>>();
                var customerSaleRepository = _iocResolver.Resolve<IRepository<CustomerSale, int>>();
                var cycleSaleRepository = _iocResolver.Resolve<IRepository<Cycle, int>>();

                int startYear = 2000;
                var lastUpdateDate = await customerSaleRepository.Query(p => p.OrderByDescending(x => x.Year)).FirstOrDefaultAsync();
                if (lastUpdateDate != null)
                {
                    startYear = Convert.ToInt32(lastUpdateDate.Year);
                }

                string sql = $@"
                    SELECT
                        c.SHORT_CODE AS CustomerCode,
                        to_char(so.ORDER_DATE, 'YYYY') AS Year,
                        to_char(so.ORDER_DATE, 'MM') AS Month,
                        to_char(so.ORDER_DATE, 'YYYYMM') AS YearMonth,
                        SUM(TOTAL) AS Amount
                    FROM VITADAIRY.SALE_ORDER so
                    INNER JOIN VITADAIRY.CUSTOMER c ON so.CUSTOMER_ID = c.CUSTOMER_ID
                    WHERE c.STATUS IN (0,1) AND so.APPROVED = 1 AND EXTRACT(YEAR FROM so.ORDER_DATE) >= {startYear}
                    GROUP BY c.SHORT_CODE, to_char(so.ORDER_DATE, 'YYYY'), to_char(so.ORDER_DATE, 'MM'),  to_char(so.ORDER_DATE, 'YYYYMM')
                    ORDER BY to_char(so.ORDER_DATE, 'YYYY'), to_char(so.ORDER_DATE, 'MM'), c.SHORT_CODE";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                int count = 0;
                foreach (DataRow item in table.Rows)
                {
                    count++;
                    string customerCode = item.Field<string>("CustomerCode");
                    string year = item.Field<string>("Year");
                    string month = item.Field<string>("Month");
                    string yearMonth = item.Field<string>("YearMonth");
                    decimal amount = item.Field<decimal>("Amount");

                    if (string.IsNullOrEmpty(customerCode)) continue;

                    var customer = await customerRepository.FirstOrDefaultAsync(p => p.Code == customerCode);

                    if (customer != null)
                    {
                        var customerSale = await customerSaleRepository.GetAll().FirstOrDefaultAsync(p => p.CustomerId == customer.Id && p.YearMonth == yearMonth);
                        if (customerSale == null)
                        {
                            customerSale = CustomerSale.Create();
                            await customerSaleRepository.InsertAsync(customerSale);
                        }

                        await customerSale.ApplyActionAsync(new CustomerSaleUpsertAction(
                            customer.Id,
                            year,
                            month,
                            yearMonth,
                            amount
                        ));
                    }
                    if (count == 100)
                    {
                        count = 0;
                        await customerSaleRepository.UnitOfWork.CommitAsync();
                    }
                }
                await customerSaleRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_CUSTOMER_SALES", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_CUSTOMER_SALES", ex);
                throw ex;
            }
        }

        private async Task SyncDistrictSync()
        {
            await _appLogger.LogInfoAsync("SYNC_DISTRICT", new { Message = "Start" });
            try
            {
                var districtRepository = _iocResolver.Resolve<IRepository<District, int>>();
                var provinceRepository = _iocResolver.Resolve<IRepository<Province, int>>();
                string sql = @"SELECT DISTINCT
                                    a.DISTRICT AS Code,
                                    a.DISTRICT_NAME AS Name,
                                    a.PROVINCE AS ProvinceId
                                    FROM VITADAIRY.AREA a
                                    WHERE a.DISTRICT IS NOT NULL";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<DistrictDto> lstDistrict = DataTableHelper.DataTableToList<DistrictDto>(table);

                var distinctDistrict = lstDistrict.GroupBy(p => p.Code).Select(p => new { Code = p.Key, Name = p.FirstOrDefault().Name, ProvinceId = p.FirstOrDefault().ProvinceId }).ToList();

                foreach (var item in distinctDistrict)
                {
                    var objProvince = await provinceRepository.FirstOrDefaultAsync(p => p.Code == item.ProvinceId);
                    if (objProvince != null)
                    {
                        var district = await districtRepository.GetAll().FirstOrDefaultAsync(p => p.Code == item.Code);
                        if (district == null)
                        {
                            district = District.Create();
                            await districtRepository.InsertAsync(district);
                        }

                        await district.ApplyActionAsync(new UpsertDistrictAction(
                            item.Code,
                            item.Name,
                            objProvince.Id
                        ));
                    }
                }
                await districtRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_DISTRICT", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_DISTRICT", ex);
                throw ex;
            }
        }

        private async Task SyncPgUserAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_CD_USER", new { Message = "Start" });

            try
            {
                var settingManager = _iocResolver.Resolve<IAppSettingManager>();
                var userAssignmentRepository = _iocResolver.Resolve<IRepository<UserAssignment, int>>();
                var branchRepository = _iocResolver.Resolve<IRepository<Branch, int>>();
                var staffRepository = _iocResolver.Resolve<IRepository<Staff, int>>();
                var userRepository = _iocResolver.Resolve<IRepository<User, int>>();
                var userRoleRepository = _iocResolver.Resolve<IRepository<UserRole, int>>();
                var roleRepository = _iocResolver.Resolve<IRepository<Role, int>>();
                var toDate = DateTime.Now;
                var lastSyncDate = await settingManager.GetAsync("SYNC_PG_DATE");
                var startDate = string.IsNullOrEmpty(lastSyncDate) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(lastSyncDate);

                string sql = $@"
                    SELECT
	                    Code,
	                    Name,
	                    Phone,
	                    Email,
	                    AreaCode,
	                    ZoneCode,
	                    ProvinceCode,
	                    CategoryCode = RTRIM(LTRIM(CategoryCode)),
	                    UpdatedTime,
	                    IsActive = IIF(Status = 'AC', 1, 0)
                    FROM SYS_Users
                    WHERE CategoryCode IN ('TP.PTKH', 'TL', 'PGAC', 'PGCD')
                    AND Code <> ''
                    AND UpdatedTime > @StartDate
                    ORDER BY UpdatedTime ASC";

                string sqlBranch = $"SELECT BranchCode = CpnyCode FROM SYS_User_Cpny WHERE UserCode = @UserCode";
                List<PgUserDto> pgUsers = new List<PgUserDto>();
                using (var connection = await _pgConnectionFactory.GetConnectionAsync())
                {
                    pgUsers = (await connection.QueryAsync<PgUserDto>(sql, new { startDate })).ToList();
                    // clear wrong data from dms
                    //foreach (var pgUser in pgUsers)
                    //{
                    //    if (pgUser.IsActive)
                    //    {
                    //        var staff = await staffRepository.FirstOrDefaultAsync(p => p.Code == pgUser.Code);
                    //        if (staff != null)
                    //        {
                    //            await staff.ApplyActionAsync(new StaffUpsertAction(
                    //                staff.Code,
                    //                staff.Name,
                    //                staff.UpdateDate,
                    //                staff.SalesOrgId,
                    //                staff.StaffTypeCode,
                    //                staff.StaffTypeCode,
                    //                staff.MobilePhone,
                    //                staff.Birthday,
                    //                staff.Email, null, null, null, false));

                    //            var user = await userRepository.GetAll().Include(p => p.Roles).FirstOrDefaultAsync(p => p.UserName == pgUser.Code);
                    //            if (user != null)
                    //            {
                    //                await userRepository.DeleteAsync(user);
                    //                var assignments = await userAssignmentRepository.GetAll().Where(p => p.UserId == user.Id).ToListAsync();
                    //                foreach (var item in assignments)
                    //                {
                    //                    await userAssignmentRepository.DeleteAsync(item);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    //await userRepository.UnitOfWork.CommitAsync();

                    foreach (var pgUser in pgUsers)
                    {
                        var user = await userRepository.GetAllIncluding(p=>p.Roles).FirstOrDefaultAsync(p => p.UserName == pgUser.Code);
                        string roleName = "";
                        switch (pgUser.CategoryCode)
                        {
                            case "TP.PTKH":
                                roleName = KmsConsts.CustomerDevelopmentManagerRole;
                                break;

                            case "TL":
                                roleName = KmsConsts.CustomerDevelopmentLeadRole;
                                break;

                            case "PGCD":
                                roleName = KmsConsts.PgRole;
                                break;

                            case "PGAC":
                                roleName = KmsConsts.PgRole;
                                break;
                        }
                        var role = await roleRepository.FirstOrDefaultAsync(p => p.RoleName == roleName);
                        var userRole = new UserRole();
                        await userRole.UpsertAsync(new UpsertUserRoleAction(role.Id));
                        if (user == null)
                        {
                            user = new User();

                            await user.ApplyActionAsync(new CreateUserAction(
                               pgUser.Code,
                               pgUser.Name ?? "",
                               PasswordManager.HashPassword("1234567"),
                               pgUser.Email ?? "",
                               pgUser.Phone ?? "",
                               null,
                               null,
                               pgUser.IsActive));

                           
                            await user.ApplyActionAsync(new CrudRoleToUserAction(
                                new List<UserRole>{
                                userRole
                                },
                                new List<UserRole>())
                            );


                            var pgBranches = (await connection.QueryAsync<string>(sqlBranch, new { UserCode = pgUser.Code })).ToList();
                            var branches = branchRepository.GetAll().Where(p => pgBranches.Contains(p.Code)).ToList();


                            await userRepository.InsertAsync(user);

                            foreach (var item in branches)
                            {
                                UserAssignment assignment = UserAssignment.Create();
                                await userAssignmentRepository.InsertAsync(assignment);
                                await assignment.ApplyActionAsync(new UpsertUserAssignmentAction(
                                    user.Id,
                                    item.SalesOrgId
                                ));
                            }

                        }
                        else
                        {
                            if (pgUser.IsActive)
                            {
                                var assignments = await userAssignmentRepository.GetAll().Where(p => p.UserId == user.Id).ToListAsync();
                                foreach (var item in assignments)
                                {
                                    await userAssignmentRepository.DeleteAsync(item);
                                }

                                await user.ApplyActionAsync(new CrudRoleToUserAction(new List<UserRole>() { userRole }, user.Roles.ToList()));

                                var pgBranches = (await connection.QueryAsync<string>(sqlBranch, new { UserCode = pgUser.Code })).ToList();
                                var branches = branchRepository.GetAll().Where(p => pgBranches.Contains(p.Code)).ToList();
                                foreach (var item in branches)
                                {
                                    UserAssignment assignment = UserAssignment.Create();
                                    await userAssignmentRepository.InsertAsync(assignment);
                                    await assignment.ApplyActionAsync(new UpsertUserAssignmentAction(
                                        user.Id,
                                        item.SalesOrgId
                                    ));
                                }
                            }
                            else if (user.Roles.Any(p=>p.Id == role.Id) || user.Roles.Count == 0)
                            {
                                await user.ApplyActionAsync(new UpsertUserAction(
                                        user.UserName,
                                        user.Name,
                                        user.Password,
                                        user.EmailAddress,
                                        user.PhoneNumber,
                                        user.Birthday,
                                        user.ExpireDate,
                                        false));
                            }
                        }
                    }
                }

                await userRepository.UnitOfWork.CommitAsync();
                await settingManager.InsertOrUpdateAsync("SYNC_PG_DATE", toDate.ToString("yyyy-MM-dd HH:mm:ss"), "");
                await _appLogger.LogInfoAsync("SYNC_CD_USER", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_CD_USER", ex);
                throw ex;
            }
            // fix pg in staff
            try
            {
                var configuration = _iocResolver.Resolve<IConfiguration>();
                using (var sqlConnection = new SqlConnection(configuration["ConnectionString"]))
                {
                    await sqlConnection.OpenAsync();
                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand(@$"
                            UPDATE staff set StaffTypeCode = '', StaffTypeName = '' from Staffs AS staff
                            WHERE EXISTS (
	                            SELECT TOP 1 * FROM Users AS [user] 
	                            JOIN UserRoles AS [userRole] ON [user].Id = [userRole].UserId
	                            JOIN Roles AS [role] ON [userRole].RoleId = [role].Id
	                            WHERE staff.UserId = [user].Id AND [user].IsActive = 1
	                            AND [RoleName] IN ('MGCD','TLCD','ACD')
                            ) AND StaffTypeCode <> ''", sqlConnection, transaction);
                            command.CommandTimeout = 3600;
                            await command.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                        }
                        catch(Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SyncProductAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_PRODUCT", new { Message = "Start" });

            try
            {
                var productRepository = _iocResolver.Resolve<IRepository<Product, int>>();
                var brandRepository = _iocResolver.Resolve<IRepository<Brand, int>>();
                var productClassRepository = _iocResolver.Resolve<IRepository<ProductClass, int>>();
                var subProductClassRepository = _iocResolver.Resolve<IRepository<SubProductClass, int>>();

                var startDate = new DateTime(2000, 1, 1);
                var lastUpdateDate = await productRepository.Query(p => p.OrderByDescending(x => x.UpdateDate)).FirstOrDefaultAsync();
                if (lastUpdateDate != null)
                {
                    startDate = lastUpdateDate.UpdateDate;
                }

                string sql = $@"
                    SELECT 
	                    latestProduct.*
                    FROM 
                    (
	                    SELECT DISTINCT p.PRODUCT_CODE
	                    FROM VITADAIRY.PRODUCT p
	                    WHERE NVL(p.UPDATE_DATE, p.CREATE_DATE) > to_date('{startDate.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                    ) distinctProduct 
                    OUTER APPLY (
	                        SELECT 
	 	                    p.PRODUCT_ID AS Id,
                            p.PRODUCT_CODE AS ProductCode,
                            p.PRODUCT_Name AS ProductName,
                            p.STATUS AS IsActive,
                            p.UOM1 AS Unit,
                            p.UOM2 AS PackUnit,
                            i1.PRODUCT_INFO_CODE AS ProductClassCode,
                            i1.PRODUCT_INFO_NAME AS ProductClassName,
                            i2.PRODUCT_INFO_CODE AS SubProductClassCode,
                            i2.PRODUCT_INFO_NAME AS SubProductClassName,
                            i.PRODUCT_INFO_CODE AS BrandCode,
                            i.PRODUCT_INFO_NAME AS BrandName,
                            p.CONVFACT AS PackSize,
                            NVL(p.UPDATE_DATE, p.CREATE_DATE) AS UpdateDate
	                    FROM VITADAIRY.PRODUCT p 
	                    LEFT JOIN   VITADAIRY.PRODUCT_INFO i1 ON p.CAT_ID = i1.PRODUCT_INFO_ID AND i1.TYPE = 1
	                    LEFT JOIN   VITADAIRY.PRODUCT_INFO i2 ON p.SUB_CAT_ID = i2.PRODUCT_INFO_ID AND i2.TYPE = 2
	                    LEFT JOIN   VITADAIRY.PRODUCT_INFO i ON p.BRAND_ID = i.PRODUCT_INFO_ID
	                    WHERE p.PRODUCT_CODE  = distinctProduct.PRODUCT_CODE 
	                    ORDER BY NVL(p.UPDATE_DATE, p.CREATE_DATE) DESC
	                    OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
                    ) latestProduct 
                    ORDER BY latestProduct.UpdateDate ASC
                ";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);

                if (table.Rows.Count > 0)
                {
                    #region Sync Brand

                    var brandTable = table.AsEnumerable()
                        .GroupBy(r => new { BrandCode = r["BrandCode"] })
                        .Select(g => g.OrderBy(r => r["BrandName"]).First())
                        .CopyToDataTable();

                    foreach (DataRow item in brandTable.Rows)
                    {
                        string brandCode = item.Field<string>("BrandCode");
                        string brandName = item.Field<string>("BrandName");

                        if (string.IsNullOrEmpty(brandCode)) continue;

                        var brand = await brandRepository.FirstOrDefaultAsync(p => p.Code == brandCode);
                        if (brand == null)
                        {
                            brand = Brand.Create();
                            await brandRepository.InsertAsync(brand);
                        }
                        await brand.ApplyActionAsync(new BrandUpsertAction(brandCode, brandName, true));
                    }

                    #endregion Sync Brand

                    #region Sync Product Class

                    var productClassTable = table.AsEnumerable()
                        .GroupBy(r => new { BrandCode = r["ProductClassCode"] })
                        .Select(g => g.OrderBy(r => r["ProductClassName"]).First())
                        .CopyToDataTable();

                    foreach (DataRow item in productClassTable.Rows)
                    {
                        string productClassCode = item.Field<string>("ProductClassCode");
                        string productClassName = item.Field<string>("ProductClassName");

                        if (string.IsNullOrEmpty(productClassCode)) continue;

                        var productClass = await productClassRepository.FirstOrDefaultAsync(p => p.Code == productClassCode);
                        if (productClass == null)
                        {
                            productClass = ProductClass.Create();
                            await productClassRepository.InsertAsync(productClass);
                        }
                        await productClass.ApplyActionAsync(new ProductClassUpsertAction(productClassCode, productClassName, "", true));
                    }

                    await productClassRepository.UnitOfWork.CommitAsync();

                    #endregion Sync Product Class

                    #region Sync Sub Product Class

                    var subProductClassTable = table.AsEnumerable()
                        .GroupBy(r => new { BrandCode = r["SubProductClassCode"] })
                        .Select(g => g.OrderBy(r => r["SubProductClassName"]).First())
                        .CopyToDataTable();

                    foreach (DataRow item in subProductClassTable.Rows)
                    {
                        string subProductClassCode = item.Field<string>("SubProductClassCode");
                        string subProductClassName = item.Field<string>("SubProductClassName");

                        if (string.IsNullOrEmpty(subProductClassCode)) continue;

                        var subProductClass = await subProductClassRepository.FirstOrDefaultAsync(p => p.Code == subProductClassCode);
                        if (subProductClass == null)
                        {
                            subProductClass = new SubProductClass();
                            await subProductClassRepository.InsertAsync(subProductClass);
                        }
                        await subProductClass.ApplyActionAsync(new SubProductClassUpsertAction(subProductClassCode, subProductClassName, true));
                    }

                    await subProductClassRepository.UnitOfWork.CommitAsync();

                    #endregion Sync Sub Product Class

                    int count = 0;
                    foreach (DataRow item in table.Rows)
                    {
                        string productCode = item.Field<string>("ProductCode");
                        if (string.IsNullOrEmpty(productCode)) continue;

                        var product = await productRepository.FirstOrDefaultAsync(p => p.Code == productCode);
                        if (product == null)
                        {
                            product = Product.Create();
                            await productRepository.InsertAsync(product);
                        }
                        string productName = item.Field<string>("ProductName");
                        string unit = item.Field<string>("Unit");
                        string packUnit = item.Field<string>("PackUnit");
                        int packSize = item.Field<int>("PackSize");
                        bool isActive = Convert.ToBoolean(item.Field<short>("IsActive"));
                        DateTime updateDate = item.Field<DateTime>("UpdateDate");

                        string productClassCode = item.Field<string>("ProductClassCode");
                        int? productClassId = null;
                        var productClass = await productClassRepository.FirstOrDefaultAsync(p => p.Code == productClassCode);
                        if (productClass != null)
                        {
                            productClassId = productClass.Id;
                        }

                        string subProductClassCode = item.Field<string>("SubProductClassCode");
                        int? subProductClassId = null;
                        var subProductClass = await subProductClassRepository.FirstOrDefaultAsync(p => p.Code == subProductClassCode);
                        if (subProductClass != null)
                        {
                            subProductClassId = subProductClass.Id;
                        }

                        string brandCode = item.Field<string>("BrandCode");
                        int? brandId = null;
                        var brand = await brandRepository.FirstOrDefaultAsync(p => p.Code == brandCode);
                        if (brand != null)
                        {
                            brandId = brand.Id;
                        }

                        await product.ApplyActionAsync(new UpsertProductAction(
                            productCode,
                            productName,
                            "",
                            unit,
                            packUnit,
                            packSize,
                            productClassId,
                            subProductClassId,
                            brandId,
                            updateDate,
                            isActive
                        ));

                        if (count == 100)
                        {
                            count = 0;
                            await productRepository.UnitOfWork.CommitAsync();
                        }
                    }
                    await productRepository.UnitOfWork.CommitAsync();
                }

                await _appLogger.LogInfoAsync("SYNC_PRODUCT", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_PRODUCT", ex);
                throw ex;
            }
        }

        private async Task SyncProductPriceAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_PRODUCT_PRICE", new { Message = "Start" });

            try
            {
                var productPriceRepository = _iocResolver.Resolve<IRepository<ProductPrice, int>>();
                var productRepository = _iocResolver.Resolve<IRepository<Product, int>>();
                var customerRepository = _iocResolver.Resolve<IRepository<Customer, int>>();

                var startDate = new DateTime(2000, 1, 1);
                var lastUpdateDate = await productPriceRepository.Query(p => p.OrderByDescending(x => x.UpdateDate)).FirstOrDefaultAsync();
                if (lastUpdateDate != null)
                {
                    startDate = lastUpdateDate.UpdateDate;
                }

                string sql = $@"
                   SELECT
	                   CAST (p.PRICE_ID AS INTEGER) AS Id,
	                    CAST (p.PRODUCT_ID AS INTEGER) AS ProductId,
                        p2.PRODUCT_CODE AS ProductCode,
	                    c.CUSTOMER_CODE AS CustomerCode,
	                    CAST (p.SHOP_ID AS INTEGER) AS SalesOrgId,
                        p.STATUS AS IsActive,
	                    p.PRICE AS Price,
	                    p.PACKAGE_PRICE AS PackagePrice,
	                    p.FROM_DATE AS FromDate,
	                    p.TO_DATE AS ToDate,
	                    NVL(p.UPDATE_DATE, p.CREATE_DATE) AS UpdateDate
                    FROM VITADAIRY.PRICE p
                    LEFT JOIN VITADAIRY.CUSTOMER c ON p.CUSTOMER_ID = c.CUSTOMER_ID
                    LEFT JOIN VITADAIRY.PRODUCT p2 ON p.PRODUCT_ID = p2.PRODUCT_ID
                    WHERE NVL(p.UPDATE_DATE, p.CREATE_DATE) >= to_date('{startDate.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                    ORDER BY NVL(p.UPDATE_DATE, p.CREATE_DATE) ASC";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                int count = 0;
                foreach (DataRow item in table.Rows)
                {
                    count++;

                    int id = (int)item.FieldOrDefault<decimal>("Id");

                    var productPrice = await productPriceRepository.FirstOrDefaultAsync(p => p.Id == id);

                    string customerCode = item.FieldOrDefault<string>("customerCode");
                    int? salesOrgId = (int?)item.FieldOrDefault<decimal?>("SalesOrgId");
                    string productCode = item.FieldOrDefault<string>("ProductCode");
                    var product = await productRepository.FirstOrDefaultAsync(p => p.Code == productCode);
                    if (product == null)
                    {
                        await _appLogger.LogErrorAsync("SYNC_PRODUCT_PRICE", new { Message = $"Product {productCode} not found" });
                        continue;
                    }
                    DateTime fromDate = item.FieldOrDefault<DateTime>("FromDate");
                    DateTime? toDate = item.FieldOrDefault<DateTime?>("ToDate");
                    DateTime updateDate = item.FieldOrDefault<DateTime>("UpdateDate");
                    decimal price = item.FieldOrDefault<decimal>("Price");
                    decimal packagePrice = item.FieldOrDefault<decimal>("PackagePrice");
                    bool isActive = Convert.ToBoolean(item.FieldOrDefault<short>("IsActive"));

                    int? customerId = null;
                    if (!string.IsNullOrEmpty(customerCode))
                    {
                        var customer = await customerRepository.FirstOrDefaultAsync(p => p.Code == customerCode);
                        if (customer != null)
                        {
                            customerId = customer.Id;
                        }
                    }

                    bool isNew = false;

                    if (productPrice == null)
                    {
                        isNew = true;
                        productPrice = new ProductPrice();
                    }

                    await productPrice.ApplyActionAsync(new ProductPriceUpsertAction(
                       id,
                       product.Id,
                       isActive,
                       price,
                       packagePrice,
                       customerId,
                       salesOrgId,
                       updateDate,
                       fromDate,
                       toDate
                    ));

                    if (isNew)
                    {
                        productPriceRepository.Insert(productPrice);
                    }

                    if (count == 100)
                    {
                        count = 0;
                        await productPriceRepository.UnitOfWork.CommitAsync();
                    }
                }
                await productPriceRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_PRODUCT_PRICE", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_PRODUCT_PRICE", ex);
                throw ex;
            }
        }

        private async Task SyncProductUnitAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_PRODUCTUNIT", new { Message = "Start" });
            try
            {
                var productUnitRepository = _iocResolver.Resolve<IRepository<ProductUnit, int>>();
                string sql = $@"
                    SELECT DISTINCT Code FROM (
                    SELECT DISTINCT UOM1 AS Code FROM VITADAIRY.PRODUCT
	                    UNION ALL
                    SELECT DISTINCT UOM2  AS Code FROM VITADAIRY.PRODUCT
                    )";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);

                foreach (DataRow item in table.Rows)
                {
                    string code = item.Field<string>("Code");
                    if (string.IsNullOrEmpty(code)) continue;

                    var productUnit = await productUnitRepository.GetAll().FirstOrDefaultAsync(p => p.Code == code);
                    if (productUnit == null)
                    {
                        productUnit = ProductUnit.Create();
                        await productUnitRepository.InsertAsync(productUnit);
                    }

                    await productUnit.ApplyActionAsync(new UpsertProductUnitAction(
                        code,
                        code,
                        true
                    ));
                }
                await productUnitRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_PRODUCTUNIT", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_PRODUCTUNIT", ex);
                throw ex;
            }
        }

        private async Task SyncProvinceAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_PROVINCE", new { Message = "Start" });
            try
            {
                var provinceRepository = _iocResolver.Resolve<IRepository<Province, int>>();
                string sql = @"SELECT DISTINCT
                        a.PROVINCE AS Code,
                        a.PROVINCE_NAME AS Name
                        FROM VITADAIRY.AREA a
                        WHERE a.PROVINCE IS NOT NULL
                        ORDER BY  a.PROVINCE";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<Province> provinces = DataTableHelper.DataTableToList<Province>(table);

                var distictProvinces = provinces.GroupBy(p => p.Code).Select(p => new { Code = p.Key, p.FirstOrDefault().Name }).ToList();
                foreach (var item in distictProvinces)
                {
                    var province = await provinceRepository.FirstOrDefaultAsync(p => p.Code == item.Code);
                    if (province == null)
                    {
                        province = Province.Create();
                        await provinceRepository.InsertAsync(province);
                    }

                    await province.ApplyActionAsync(new UpsertProvinceAction(
                        item.Code,
                        item.Name
                    ));
                }
                await provinceRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_PROVINCE", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_PROVINCE", ex);
                throw ex;
            }
        }

        private async Task SyncSalesOrgAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_SALES_ORG", new { Message = "Start" });
            try
            {
                var salesOrgRepository = _iocResolver.Resolve<IRepository<SalesOrg, int>>();
                string sql = @"SELECT
                                s.SHOP_ID AS Id,
                                s.SHOP_CODE AS Code,
                                s.SHOP_NAME AS Name,
                                s.PARENT_SHOP_ID AS ParentId,
                                st.SHOP_TYPE_ID AS TypeId,
                                st.NAME AS TypeName,
                                NVL(s.UPDATE_DATE, s.CREATE_DATE) AS UpdateDate
                                FROM VITADAIRY.SHOP s
                                INNER JOIN VITADAIRY.SHOP_TYPE st ON s.SHOP_TYPE_ID = st.SHOP_TYPE_ID
                            ";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<SalesOrg> lstSalesOrg = DataTableHelper.DataTableToList<SalesOrg>(table);
                foreach (var item in lstSalesOrg)
                {
                    var salesOrg = await salesOrgRepository.GetAll().FirstOrDefaultAsync(p => p.Id == item.Id);
                    if (salesOrg == null)
                    {
                        salesOrg = SalesOrg.Create();
                        await salesOrg.ApplyActionAsync(new UpsertSalesOrgAction(
                            item.Id,
                            item.Code,
                            item.Name,
                            item.ParentId,
                            item.TypeId,
                            item.TypeName,
                            item.UpdateDate
                        ));
                        await salesOrgRepository.InsertAsync(salesOrg);
                    }
                    else
                    {
                        await salesOrg.ApplyActionAsync(new UpsertSalesOrgAction(
                            item.Id,
                            item.Code,
                            item.Name,
                            item.ParentId,
                            item.TypeId,
                            item.TypeName,
                            item.UpdateDate
                        ));
                    }
                }
                await salesOrgRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_SALES_ORG", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_SALES_ORG", ex);
                throw ex;
            }
        }

        private async Task SyncStaffAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_STAFF", new { Message = "Start" });
            try
            {
                var settingManager = _iocResolver.Resolve<IAppSettingManager>();
                var userRepository = _iocResolver.Resolve<IRepository<User, int>>();
                var userRoleRepository = _iocResolver.Resolve<IRepository<UserRole, int>>();
                var staffRepository = _iocResolver.Resolve<IRepository<Staff, int>>();
                var salesOrgRepository = _iocResolver.Resolve<IRepository<SalesOrg, int>>();
                var roleRepository = _iocResolver.Resolve<IRepository<Role, int>>();
                var areaRepository = _iocResolver.Resolve<IRepository<Area, int>>();
                var zoneRepository = _iocResolver.Resolve<IRepository<Zone, int>>();
                var customerRepository = _iocResolver.Resolve<IRepository<Customer, int>>();
                var branchRepository = _iocResolver.Resolve<IRepository<Branch, int>>();
                var provinceRepository = _iocResolver.Resolve<IRepository<Province, int>>();
                var districtRepository = _iocResolver.Resolve<IRepository<District, int>>();
                var wardRepository = _iocResolver.Resolve<IRepository<Ward, int>>();
                var lastSyncDate = await settingManager.GetAsync("SYNC_STAFF_DATE");
                var toDate = DateTime.Now;
                var fromDate = string.IsNullOrEmpty(lastSyncDate) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(lastSyncDate);

                string sql = $@"SELECT
                                s.STAFF_CODE AS Code,
                                s.STAFF_NAME AS Name,
                                s.SHOP_ID AS SalesOrgId,
                                CASE WHEN st.PREFIX IS NULL THEN 'ADMIN' ELSE st.PREFIX  END AS StaffTypeCode,
                                st.NAME AS StaffTypeName,
                                NVL(s.UPDATE_DATE, s.CREATE_DATE) AS UpdateDate,
                                s.EMAIL,
                                s.MOBILEPHONE,
                                s.BIRTHDAY,
                                CAST(s.STATUS AS SMALLINT) AS IsActive
                            FROM VITADAIRY.STAFF s
                            INNER JOIN VITADAIRY.STAFF_TYPE st ON s.STAFF_TYPE_ID = st.STAFF_TYPE_ID
                            INNER JOIN VITADAIRY.SHOP h ON s.SHOP_ID = h.SHOP_ID
                            WHERE st.PREFIX IN ('RSM', 'ASM', 'GSBH')
                            AND NVL(s.UPDATE_DATE, s.CREATE_DATE) >= to_date('{fromDate.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                            AND NVL(s.UPDATE_DATE, s.CREATE_DATE) < to_date('{toDate.ToString("yyyy-MM-dd HH:mm:ss")}','YYYY-MM-DD HH24:MI:SS')
                            ORDER BY NVL(s.UPDATE_DATE, s.CREATE_DATE) ASC";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);

                List<int> customerIds = new List<int>();
                foreach (DataRow item in table.Rows)
                {
                    string staffCode = item.Field<string>("Code");

                    if (string.IsNullOrEmpty(staffCode)) continue;

                    string staffTypeCode = item.Field<string>("StaffTypeCode");
                    string staffTypeName = item.Field<string>("StaffTypeName");
                    string staffType = staffTypeCode;

                    switch (staffTypeCode)
                    {
                        case "GSBH":
                            staffType = "SS";
                            break;

                        case "NVBH":
                            staffType = "SR";
                            break;
                    }

                    var role = await roleRepository.FirstOrDefaultAsync(p => p.RoleName == staffType);

                    string staffName = item.Field<string>("Name");
                    string email = item.Field<string>("Email");
                    string mobilePhone = item.Field<string>("MobilePhone");
                    DateTime? birthday = item.Field<DateTime?>("Birthday");
                    DateTime updateDate = item.Field<DateTime>("UpdateDate");
                    bool isActive = Convert.ToBoolean(item.Field<decimal>("IsActive"));

                    int? salesOrgId = null;
                    if (item.Field<decimal?>("SalesOrgId").HasValue)
                    {
                        salesOrgId = (int)item.Field<decimal?>("SalesOrgId").Value;
                    }

                    var user = await userRepository.GetAllIncluding(p=>p.Roles).FirstOrDefaultAsync(p => p.UserName == staffCode);

                    var userRole = new UserRole();
                    await userRole.UpsertAsync(new UpsertUserRoleAction(role.Id));

                    if (user == null)
                    {
                        user = new User();

                        await user.ApplyActionAsync(new CreateUserAction(
                            staffCode,
                            staffName,
                            PasswordManager.HashPassword(staffCode),
                            email,
                            mobilePhone,
                            birthday,
                            null,
                            isActive));

                        await userRepository.InsertAsync(user);

                        await user.ApplyActionAsync(new CrudRoleToUserAction(
                            new List<UserRole>{
                                userRole
                            },
                            new List<UserRole>())
                        );
                    }
                    else
                    {
                        if (isActive)
                        {
                            await user.ApplyActionAsync(new UpsertUserAction(
                           staffCode,
                           staffName,
                           user.Password,
                           email,
                           mobilePhone,
                           birthday,
                           user.ExpireDate,
                           isActive));

                            var currentRole = user.Roles.FirstOrDefault();
                            if (currentRole == null || currentRole.RoleId != userRole.RoleId)
                            {
                                await user.ApplyActionAsync(new CrudRoleToUserAction(
                                      new List<UserRole>{
                                        userRole
                                      },
                                      user.Roles.ToList())
                                  );
                            }
                        }
                        else if (user.Roles.Count == 0 || user.Roles.Any(p=>p.RoleId == role.Id))
                        {
                            await user.ApplyActionAsync(new UpsertUserAction(
                                       user.UserName,
                                       user.Name,
                                       user.Password,
                                       user.EmailAddress,
                                       user.PhoneNumber,
                                       user.Birthday,
                                       user.ExpireDate,
                                       false));
                        }
                    }

                    var salesOrg = await salesOrgRepository.GetAll().FirstOrDefaultAsync(p => p.Id == salesOrgId);
                    if (salesOrg != null)
                    {
                        var staff = await staffRepository.GetAll().FirstOrDefaultAsync(p => p.Code == staffCode);
                        if (staff == null)
                        {
                            staff = Staff.Create();
                            await staffRepository.InsertAsync(staff);
                        }
                        else
                        {
                            var ids = customerRepository
                                .GetAllList(p => p.SalesSupervisorStaffId == staff.Id || p.AsmStaffId == staff.Id || p.RsmStaffId == staff.Id)
                                .Select(p => p.Id).ToList();
                            customerIds.AddRange(ids);
                        }

                        int? areaId = null;
                        int? zoneId = null;

                        if (staffType == KmsConsts.RsmRole)
                        {
                            var zoneOrg = _dbContext.SalesOrgs.FromSqlRaw(@$"
                                WITH CTE AS
                                (
	                                SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    WHERE  SalesOrgs.Id = {salesOrgId}
                                    UNION ALL

                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    INNER JOIN CTE     ON SalesOrgs.id = CTE.ParentId
                                )
                                SELECT *
                                FROM   CTE
                                WHERE [TypeId] = 1144
                            ").ToList().FirstOrDefault();

                            if (zoneOrg != null)
                            {
                                var zone = await zoneRepository.FirstOrDefaultAsync(p => p.SalesOrgId == zoneOrg.Id);
                                if (zone != null)
                                {
                                    zoneId = zone.Id;
                                }
                            }
                        }
                        else
                        {
                            var areaOrg = _dbContext.SalesOrgs.FromSqlRaw(@$"
                                WITH CTE AS
                                (
	                                SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    WHERE  SalesOrgs.Id = {salesOrgId}
                                    UNION ALL

                                    SELECT SalesOrgs.*
                                    FROM   SalesOrgs
                                    INNER JOIN CTE     ON SalesOrgs.id = CTE.ParentId
                                )
                                SELECT *
                                FROM   CTE
                                WHERE [TypeId] = 1145
                            ").ToList().FirstOrDefault();

                            if (areaOrg != null)
                            {
                                var area = await areaRepository.FirstOrDefaultAsync(p => p.SalesOrgId == areaOrg.Id);
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    zoneId = area.ZoneId;
                                }
                            }
                        }

                        await staff.ApplyActionAsync(new StaffUpsertAction(
                            staffCode,
                            staffName,
                            updateDate,
                            salesOrg.Id,
                            staffType,
                            staffTypeName,
                            mobilePhone,
                            birthday,
                            email,
                            user.Id,
                            areaId,
                            zoneId,
                            isActive
                        ));
                    }
                }
                customerIds = customerIds.Distinct().ToList();
                /// update Customer by change staff
                foreach (var custId in customerIds)
                {
                    var customer = await customerRepository.FindAsync(custId);

                    int? rsmStaffId = null;
                    int? asmStaffId = null;

                    if (customer.AreaId.HasValue)
                    {
                        var area = await areaRepository.FirstOrDefaultAsync(p => p.Id == customer.AreaId);
                        if (area != null)
                        {
                            var asmStaff = await staffRepository.FirstOrDefaultAsync(p => p.SalesOrgId == area.SalesOrgId && p.StaffTypeCode == "ASM" && p.IsActive == true);
                            if (asmStaff != null)
                            {
                                asmStaffId = asmStaff.Id;
                            }

                            var zone = await zoneRepository.FirstOrDefaultAsync(p => p.Id == area.ZoneId);
                            var rsmStaff = await staffRepository.FirstOrDefaultAsync(p => p.SalesOrgId == zone.SalesOrgId && p.StaffTypeCode == "RSM" && p.IsActive == true);
                            if (rsmStaff != null)
                            {
                                rsmStaffId = rsmStaff.Id;
                            }
                        }
                    }

                    await customer.ApplyActionAsync(new CustomerUpsertAction(
                        false,
                        customer.Code,
                        customer.Name,
                        customer.BranchId,
                        customer.ContactName,
                        customer.MobilePhone ?? "",
                        customer.Phone ?? "",
                        customer.Email,
                        customer.ChannelCode,
                        customer.ChannelName,
                        customer.HouseNumber,
                        customer.Street,
                        customer.Address,
                        customer.WardId,
                        customer.DistrictId,
                        customer.ProvinceId,
                        customer.IsActive,
                        customer.Lat,
                        customer.Lng,
                        customer.Birthday,
                        customer.UpdateDate,
                        customer.AreaId,
                        customer.ZoneId,
                        rsmStaffId,
                        asmStaffId,
                        customer.SalesSupervisorStaffId
                    ));
                }

                await staffRepository.UnitOfWork.CommitAsync();
                await settingManager.InsertOrUpdateAsync("SYNC_STAFF_DATE", toDate.ToString("yyyy-MM-dd HH:mm:ss"), "");
                await _appLogger.LogInfoAsync("SYNC_STAFF", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_STAFF", ex);
                throw ex;
            }
        }

        private async Task SyncWardAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_WARD", new { Message = "Start" });
            try
            {
                var districtRepository = _iocResolver.Resolve<IRepository<District, int>>();
                var wardRepository = _iocResolver.Resolve<IRepository<Ward, int>>();
                var startDate = new DateTime(2000, 1, 1);
                var maxDate = await wardRepository.Query(p => p.OrderByDescending(x => x.UpdateDate)).FirstOrDefaultAsync();
                if (maxDate != null)
                {
                    startDate = maxDate.UpdateDate;
                }
                string sql = $@"SELECT DISTINCT
                                    a.PRECINCT AS Code,
                                    a.PRECINCT_NAME AS Name,
                                    a.DISTRICT AS DistrictId,
                                    nvl(a.UPDATE_DATE, a.CREATE_DATE) AS UpdateDate
                                    FROM VITADAIRY.AREA a
                                    WHERE a.PRECINCT IS NOT NULL AND NVL(a.UPDATE_DATE, a.CREATE_DATE) > to_date('{startDate.ToString("yyyy - MM - dd HH: mm: ss")}','YYYY-MM-DD HH24:MI:SS')
                                    ORDER BY nvl(a.UPDATE_DATE, a.CREATE_DATE) ASC";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<WardDto> lstWard = DataTableHelper.DataTableToList<WardDto>(table);
                int count = 0;
                var distinctWard = lstWard.GroupBy(p => p.Code).Select(p => new { Code = p.Key, Name = p.FirstOrDefault().Name, DistrictId = p.FirstOrDefault().DistrictId, UpdateDate = p.FirstOrDefault().UpdateDate }).ToList();
                foreach (var item in distinctWard)
                {
                    var objDistrict = await districtRepository.FirstOrDefaultAsync(p => p.Code == item.DistrictId);
                    if (objDistrict != null)
                    {
                        count++;
                        var ward = await wardRepository.GetAll().FirstOrDefaultAsync(p => p.Code == item.Code);
                        if (ward == null)
                        {
                            ward = Ward.Create();
                            await wardRepository.InsertAsync(ward);
                        }

                        await ward.ApplyActionAsync(new UpsertWardAction(
                            item.Code,
                            item.Name,
                            objDistrict.Id,
                            DateTime.Parse(item.UpdateDate)
                        ));
                    }
                    if (count == 100)
                    {
                        count = 0;
                        await wardRepository.UnitOfWork.CommitAsync();
                    }
                }
                await wardRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_WARD", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_WARD", ex);
                throw ex;
            }
        }

        private async Task SyncZoneAsync()
        {
            await _appLogger.LogInfoAsync("SYNC_ZONE", new { Message = "Start" });
            try
            {
                var zoneRepository = _iocResolver.Resolve<IRepository<Zone, int>>();
                string sql = @"
                    SELECT
	                    SHOP_ID AS SalesOrgId,
                        SHOP_CODE AS Code,
                        SHOP_NAME AS Name
                    FROM VITADAIRY.SHOP
                    WHERE SHOP_TYPE_ID = 1144";

                DataTable table = _oracleDataAccess.ExecuteQuery(sql);
                List<Zone> lstZone = DataTableHelper.DataTableToList<Zone>(table);
                var distictZones = lstZone.GroupBy(p => p.Code).Select(p => new { Code = p.Key, p.FirstOrDefault().Name }).ToList();

                foreach (var item in lstZone)
                {
                    var zone = await zoneRepository.GetAll().FirstOrDefaultAsync(p => p.SalesOrgId == item.SalesOrgId);
                    if (zone == null)
                    {
                        zone = Zone.Create();
                        await zoneRepository.InsertAsync(zone);
                    }

                    await zone.ApplyActionAsync(new UpsertZoneAction(
                        item.Code,
                        item.Name,
                        item.SalesOrgId
                    ));
                }
                await zoneRepository.UnitOfWork.CommitAsync();
                await _appLogger.LogInfoAsync("SYNC_ZONE", new { Message = "Finish" });
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_ZONE", ex);
                throw ex;
            }
        }
    }
}