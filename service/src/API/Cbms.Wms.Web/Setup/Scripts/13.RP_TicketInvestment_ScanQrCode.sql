IF OBJECT_ID(N'RP_TicketInvestment_ScanQrCode') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_ScanQrCode
END
GO

CREATE PROCEDURE [dbo].[RP_TicketInvestment_ScanQrCode]
@UserId int,
@FromDate DateTime,
@ToDate DateTime,
@StaffId int = NULL
AS

IF @StaffId IS NULL 
BEGIN
	SELECT @StaffId = Id FROM Staffs WHERE UserId = @UserId
END

IF @StaffId IS NOT NULL
BEGIN

	DECLARE @SalesOrgId INT;
	SELECT @SalesOrgId = SalesOrgId FROM Staffs WHERE Id = @StaffId;

	WITH CTE AS
	(
		SELECT SalesOrgs.*
		FROM   SalesOrgs
		WHERE Id = @SalesOrgId

		UNION ALL

		SELECT SalesOrgs.*
		FROM   SalesOrgs
		INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
	)
	
	SELECT 
		  FromDate = FORMAT(@FromDate,'dd/MM/yyyy') 
		, ToDate = FORMAT(@ToDate,'dd/MM/yyyy') 
		, ZoneName = zone.Name
		, AreaName = area.Name
		, ProvinceName = province.Name
		, RSMCode = staffRSM.Code
		, RSMName = staffRSM.Name
		, ASMCode = staffASM.Code
		, ASMName = staffASM.Name
		, SSCode = staffSS.Code
		, SSName = staffSS.Name
		, BranchCode = branch.Code
		, BranchName = branch.Name		
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, TicketInvestmentCode =  ticketInvest.Code
		, StatusName = CASE 
							WHEN ticketInvest.Status = 10 THEN N'Đề xuất'
							WHEN ticketInvest.Status = 20 THEN N'Không Xác Nhận Yêu Cầu'
							WHEN ticketInvest.Status = 30 THEN N'Xác Nhận Yêu Cầu'
							WHEN ticketInvest.Status = 40 THEN N'Xác Nhận PYC Hợp Lệ 1'
							WHEN ticketInvest.Status = 50 THEN N'PYC Không Hợp Lệ 1'
							WHEN ticketInvest.Status = 60 THEN N'Xác Nhận PYC Hợp Lệ 2'
							WHEN ticketInvest.Status = 70 THEN N'PYC Không Hợp Lệ 2'
							WHEN ticketInvest.Status = 80 THEN N'Xác Nhận Đầu Tư'
							WHEN ticketInvest.Status = 90 THEN N'Không Xác Nhận Đầu Tư'
							WHEN ticketInvest.Status = 100 THEN N'Trade Duyệt Đầu Tư'
							WHEN ticketInvest.Status = 110 THEN N'Trade Không Duyệt Đầu Tư'
							WHEN ticketInvest.Status = 120 THEN N'Đã Duyệt'
							WHEN ticketInvest.Status = 130 THEN N'Hủy Duyệt Đầu Tư'
							WHEN ticketInvest.Status = 140 THEN N'Đang Thực Hiện'
							WHEN ticketInvest.Status = 150 THEN N'Đã Tổ Chức'
							WHEN ticketInvest.Status = 160 THEN N'Đã Nghiệm Thu'
							WHEN ticketInvest.Status = 170 THEN N'Đã Quyết Toán'
					END
		, ProductCode
		, ProductName
		, QrCode
		, UnitPrice
		, Quantity = 1.0
		, ScanTime = b.CreationTime
	FROM TicketInvestments AS ticketInvest
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId	
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		OUTER APPLY (
				SELECT [order].CreationTime, [detail].ProductCode, detail.[ProductName], detail.UnitPrice, [detail].QrCode
				FROM Orders [order]
				INNER JOIN OrderDetails [detail] ON [order].Id = [detail].OrderId
				WHERE [order].CustomerId = ticketInvest.CustomerId and [order].TicketInvestmentId = ticketInvest.Id

				UNION 

				SELECT salesItem.CreationTime, ProductCode = product.Code, ProductName = product.Name, UnitPrice = salesItem.Price, salesItem.QrCode
				FROM CustomerSalesItems as salesItem
				INNER JOIN Products product ON salesItem.ProductId = product.Id
				WHERE salesItem.CustomerId = ticketInvest.CustomerId AND salesItem.IsUsing = 0 AND salesItem.TicketInvestmentId = ticketInvest.Id

		) AS b
	WHERE  b.CreationTime >= @FromDate 
	AND b.CreationTime <= @ToDate           
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	AND QrCode IS NOT NULL

END
ELSE 
BEGIN
	WITH CTE AS
	(
		SELECT SalesOrgs.*
        FROM   SalesOrgs
        INNER JOIN UserAssignments  ON SalesOrgs.Id = UserAssignments.SalesOrgId
	    WHERE UserAssignments.UserId = @UserId

        UNION ALL

        SELECT SalesOrgs.*
        FROM   SalesOrgs
        INNER JOIN CTE ON SalesOrgs.ParentId = CTE.Id
	)

	SELECT 
		  FromDate = FORMAT(@FromDate,'dd/MM/yyyy') 
		, ToDate = FORMAT(@ToDate,'dd/MM/yyyy') 
		, ZoneName = zone.Name
		, AreaName = area.Name
		, ProvinceName = province.Name
		, RSMCode = staffRSM.Code
		, RSMName = staffRSM.Name
		, ASMCode = staffASM.Code
		, ASMName = staffASM.Name
		, SSCode = staffSS.Code
		, SSName = staffSS.Name
		, BranchCode = branch.Code
		, BranchName = branch.Name		
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, TicketInvestmentCode =  ticketInvest.Code
		, StatusName = 
				CASE WHEN ticketInvest.Status = 10 THEN N'Đề xuất'
					WHEN ticketInvest.Status = 20 THEN N'Không Xác Nhận Yêu Cầu'
					WHEN ticketInvest.Status = 30 THEN N'Xác Nhận Yêu Cầu'
					WHEN ticketInvest.Status = 40 THEN N'Xác Nhận PYC Hợp Lệ 1'
					WHEN ticketInvest.Status = 50 THEN N'PYC Không Hợp Lệ 1'
					WHEN ticketInvest.Status = 60 THEN N'Xác Nhận PYC Hợp Lệ 2'
					WHEN ticketInvest.Status = 70 THEN N'PYC Không Hợp Lệ 2'
					WHEN ticketInvest.Status = 80 THEN N'Xác Nhận Đầu Tư'
					WHEN ticketInvest.Status = 90 THEN N'Không Xác Nhận Đầu Tư'
					WHEN ticketInvest.Status = 100 THEN N'Trade Duyệt Đầu Tư'
					WHEN ticketInvest.Status = 110 THEN N'Trade Không Duyệt Đầu Tư'
					WHEN ticketInvest.Status = 120 THEN N'Đã Duyệt'
					WHEN ticketInvest.Status = 130 THEN N'Hủy Duyệt Đầu Tư'
					WHEN ticketInvest.Status = 140 THEN N'Đang Thực Hiện'
					WHEN ticketInvest.Status = 150 THEN N'Đã Tổ Chức'
					WHEN ticketInvest.Status = 160 THEN N'Đã Nghiệm Thu'
					WHEN ticketInvest.Status = 170 THEN N'Đã Quyết Toán'
				END
			, ProductCode
			, ProductName
			, QrCode
			, UnitPrice
			, Quantity = 1.0
			, ScanTime = b.CreationTime
	FROM TicketInvestments AS ticketInvest
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId	
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		OUTER APPLY (
			SELECT [order].CreationTime, [detail].ProductCode, detail.[ProductName], detail.UnitPrice, [detail].QrCode
			FROM Orders [order]
			INNER JOIN OrderDetails [detail] ON [order].Id = [detail].OrderId
			WHERE [order].CustomerId = ticketInvest.CustomerId and [order].TicketInvestmentId = ticketInvest.Id

			UNION 

			SELECT salesItem.CreationTime, ProductCode = product.Code, ProductName = product.Name, UnitPrice = salesItem.Price, salesItem.QrCode
			FROM CustomerSalesItems as salesItem
			INNER JOIN Products product ON salesItem.ProductId = product.Id
			WHERE salesItem.CustomerId = ticketInvest.CustomerId AND salesItem.IsUsing = 0 AND salesItem.TicketInvestmentId = ticketInvest.Id
		) AS b
	WHERE  b.CreationTime >= @FromDate 
	AND b.CreationTime <= @ToDate        
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	AND QrCode IS NOT NULL
END