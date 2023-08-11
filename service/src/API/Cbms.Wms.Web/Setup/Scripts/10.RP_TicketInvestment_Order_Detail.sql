IF OBJECT_ID(N'RP_TicketInvestment_Order_Detail') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_Order_Detail
END
GO
CREATE PROCEDURE RP_TicketInvestment_Order_Detail
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

	DECLARE @SalesOrgId INT
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
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
		, BranchCode = branch.Code
		, BranchName = branch.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, TicketInvestmentCode = ticketInvestment.Code
		, OrderDate = FORMAT(orders.OrderDate,'dd/MM/yyyy') 
		, CustName = customer.Name
		, QrCode = orderDetail.QrCode
		, SpoonCode = orderDetail.SpoonCode
		, ProductCode = orderDetail.ProductCode
		, ProductName = orderDetail.ProductName
		, API = orderDetail.Api
		, Quantity = orderDetail.Quantity
		, UnitPrice = orderDetail.UnitPrice
		, Points = orderDetail.Points 
		, CreationTime = FORMAT(orders.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
	FROM Orders AS orders
	INNER JOIN OrderDetails AS orderDetail ON orderDetail.OrderId = orders.Id 
	INNER JOIN Customers AS customer ON orders.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN TicketInvestments as ticketInvestment on orders.TicketInvestmentId = ticketInvestment.Id
	LEFT JOIN Staffs AS staffSS ON staffSS.SalesOrgId = branch.SalesOrgId AND staffSS.StaffTypeCode = 'SS' AND staffSS.IsActive = 1
	LEFT JOIN Staffs AS staffASM ON staffASM.AreaId = customer.AreaId AND staffASM.StaffTypeCode = 'ASM' AND staffASM.IsActive = 1
	LEFT JOIN Staffs AS staffRSM ON staffRSM.ZoneId = customer.ZoneId AND staffRSM.StaffTypeCode = 'RSM' AND staffRSM.IsActive = 1
	LEFT JOIN Areas AS area ON area.Id = customer.AreaId
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	WHERE  Orders.CreationTime >= @FromDate 
	AND Orders.CreationTime <= @ToDate              
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	ORDER BY Orders.CreationTime, customer.Code
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
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
		, BranchCode = branch.Code
		, BranchName = branch.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, TicketInvestmentCode = ticketInvestment.Code
		, OrderDate = FORMAT(orders.OrderDate,'dd/MM/yyyy') 
		, CustName = customer.Name
		, QrCode = orderDetail.QrCode
		, SpoonCode = orderDetail.SpoonCode
		, ProductCode = orderDetail.ProductCode
		, ProductName = orderDetail.ProductName
		, API = orderDetail.Api
		, Quantity = orderDetail.Quantity
		, UnitPrice = orderDetail.UnitPrice
		, Points = orderDetail.Points 
		, CreationTime = FORMAT(orders.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
	FROM Orders AS orders
	INNER JOIN OrderDetails AS orderDetail ON orderDetail.OrderId = orders.Id 
	INNER JOIN Customers AS customer ON orders.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN TicketInvestments as ticketInvestment on orders.TicketInvestmentId = ticketInvestment.Id
	LEFT JOIN Staffs AS staffSS ON staffSS.SalesOrgId = branch.SalesOrgId AND staffSS.StaffTypeCode = 'SS' AND staffSS.IsActive = 1
	LEFT JOIN Staffs AS staffASM ON staffASM.AreaId = customer.AreaId AND staffASM.StaffTypeCode = 'ASM' AND staffASM.IsActive = 1
	LEFT JOIN Staffs AS staffRSM ON staffRSM.ZoneId = customer.ZoneId AND staffRSM.StaffTypeCode = 'RSM' AND staffRSM.IsActive = 1
	LEFT JOIN Areas AS area ON area.Id = customer.AreaId
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	WHERE  Orders.CreationTime >= @FromDate 
	AND Orders.CreationTime <= @ToDate              
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY Orders.CreationTime, customer.Code
END
