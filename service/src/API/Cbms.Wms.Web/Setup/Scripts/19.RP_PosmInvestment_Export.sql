IF OBJECT_ID(N'RP_PosmInvestment_Export') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_PosmInvestment_Export
END

GO

CREATE PROC [dbo].[RP_PosmInvestment_Export]
@UserId int,
@FromDate DateTime,
@ToDate DateTime,
@StaffId int = NULL,
@Status int
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
		, BranchCode = branch.Code
		, BranchName = branch.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ShopLocation = cl.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, CreationTime = FORMAT(posmInvest.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
		, CurrentSalesAmount = posmInvest.CurrentSalesAmount
		, InvestmentAmount = posmInvest.InvestmentAmount
		, CommitmentAmount = posmInvest.CommitmentAmount
		, SetupPlanDate = FORMAT(itemInvest.SetupPlanDate,'yyyy-MM-ddTHH:mm:ss')
		, InvestmentCode = posmInvest.Code
		, PosmItemId = itemInvest.PosmItemId
		, PosmItemCode = item.Code
		, PosmItemName = item.Name
		, PosmCatalogId = itemInvest.PosmCatalogId
		, PosmCatalogCode = cata.Code
		, PosmCatalogName = cata.Name
		, PosmClassId = itemInvest.PosmClassId
		, PosmClassCode = clazz.Code
		, PosmClassName = clazz.Name
		, Qty = itemInvest.Qty
		, RequestType = CASE 
							WHEN itemInvest.RequestType = 1THEN N'Đầu tư mới'
							WHEN itemInvest.RequestType = 2THEN N'Sửa chữa'
						END
		, RequestReason = itemInvest.RequestReason
		, UnitType = CASE 
						WHEN item.UnitType = 1 THEN N'Cái'
						WHEN item.UnitType = 2 THEN N'Mét'
						WHEN item.UnitType = 3 THEN N'Mét vuông'
					END
		, UnitPrice = itemInvest.UnitPrice
		, TotalCost = itemInvest.TotalCost
		, itemInvest.Size
		, itemInvest.Width
		, itemInvest.SideWidth1
		, itemInvest.SideWidth2
		, itemInvest.Height
		, itemInvest.Depth
		, itemInvest.PosmValue
		, Price = itemInvest.PosmValue * itemInvest.UnitPrice
		, ActualPrice = itemInvest.PosmValue * itemInvest.ActualUnitPrice
		, Spec = CASE 
					WHEN item.CalcType = 1 THEN
						N'Rộng [' + FORMAT(itemInvest.Width, '#,###', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###', 'vi') + ']'
					WHEN item.CalcType = 2 THEN
						N'Cao [' + FORMAT(itemInvest.Height, '#,###', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###', 'vi') + ']'
					WHEN item.CalcType = 3 THEN
						N'Rộng [' + FORMAT(itemInvest.Width, '#,###', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###', 'vi') + ']'
					WHEN item.CalcType = 4 THEN
						N'(Cạnh 1 [' + FORMAT(itemInvest.SideWidth1, '#,###', 'vi') + '] + ' + N'Cạnh 2 [' +  FORMAT(itemInvest.SideWidth2, '#,###', 'vi') + ']) x ' + N'Mặt rộng [' +  FORMAT(itemInvest.Width, '#,###', 'vi') + ']'
					ELSE ''
				 END 
	FROM PosmInvestments AS posmInvest
	INNER JOIN PosmInvestmentItems AS itemInvest ON posmInvest.Id = itemInvest.PosmInvestmentId
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN CustomerLocations AS cl ON posmInvest.CustomerLocationId = cl.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN PosmItems AS item ON itemInvest.PosmItemId = item.Id
	INNER JOIN PosmClasses AS clazz ON itemInvest.PosmClassId = clazz.Id
	INNER JOIN PosmCatalogs AS cata ON itemInvest.PosmCatalogId = cata.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	WHERE  posmInvest.CreationTime >= @FromDate 
	AND posmInvest.CreationTime <= @ToDate       
	AND itemInvest.Status = ISNULL(@Status, itemInvest.Status)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)   
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	ORDER BY posmInvest.CreationTime, customer.Code
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
		, BranchCode = branch.Code
		, BranchName = branch.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ShopLocation = cl.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, CreationTime = FORMAT(posmInvest.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
		, CurrentSalesAmount = posmInvest.CurrentSalesAmount
		, InvestmentAmount = posmInvest.InvestmentAmount
		, CommitmentAmount = posmInvest.CommitmentAmount
		, SetupPlanDate = FORMAT(itemInvest.SetupPlanDate,'yyyy-MM-ddTHH:mm:ss')
		, InvestmentCode = posmInvest.Code
		, PosmItemId = itemInvest.PosmItemId
		, PosmItemCode = item.Code
		, PosmItemName = item.Name
		, PosmCatalogId = itemInvest.PosmCatalogId
		, PosmCatalogCode = cata.Code
		, PosmCatalogName = cata.Name
		, PosmClassId = itemInvest.PosmClassId
		, PosmClassCode = clazz.Code
		, PosmClassName = clazz.Name
		, Qty = itemInvest.Qty
		, RequestType = CASE 
							WHEN itemInvest.RequestType = 1THEN N'Đầu tư mới'
							WHEN itemInvest.RequestType = 2THEN N'Sửa chữa'
						END
		, RequestReason = itemInvest.RequestReason
		, UnitType = CASE 
						WHEN item.UnitType = 1 THEN N'Cái'
						WHEN item.UnitType = 2 THEN N'Mét'
						WHEN item.UnitType = 3 THEN N'Mét vuông'
					END
		, UnitPrice = itemInvest.UnitPrice
		, ActualUnitPrice = itemInvest.ActualUnitPrice
		, TotalCost = itemInvest.TotalCost
		, ActualTotalCost = itemInvest.ActualTotalCost
		, itemInvest.Size
		, itemInvest.Width
		, itemInvest.SideWidth1
		, itemInvest.SideWidth2
		, itemInvest.Height
		, itemInvest.Depth
		, itemInvest.PosmValue
		, Price = itemInvest.PosmValue * itemInvest.UnitPrice
		, ActualPrice = itemInvest.PosmValue * itemInvest.ActualUnitPrice
		, Spec = CASE 
				WHEN item.CalcType = 1 THEN
					N'Rộng [' + FORMAT(itemInvest.Width, '#,###', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###', 'vi') + ']'
				WHEN item.CalcType = 2 THEN
					N'Cao [' + FORMAT(itemInvest.Height, '#,###', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###', 'vi') + ']'
				WHEN item.CalcType = 3 THEN
					N'Rộng [' + FORMAT(itemInvest.Width, '#,###', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###', 'vi') + ']'
				WHEN item.CalcType = 4 THEN
					N'(Cạnh 1 [' + FORMAT(itemInvest.SideWidth1, '#,###', 'vi') + '] + ' + N'Cạnh 2 [' +  FORMAT(itemInvest.SideWidth2, '#,###', 'vi') + ']) x ' + N'Mặt rộng [' +  FORMAT(itemInvest.Width, '#,###', 'vi') + ']'
				ELSE ''
				 END 
	FROM PosmInvestments AS posmInvest
	INNER JOIN PosmInvestmentItems AS itemInvest ON posmInvest.Id = itemInvest.PosmInvestmentId
	INNER JOIN CustomerLocations AS cl ON posmInvest.CustomerLocationId = cl.Id
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN PosmItems AS item ON itemInvest.PosmItemId = item.Id
	INNER JOIN PosmClasses AS clazz ON itemInvest.PosmClassId = clazz.Id
	INNER JOIN PosmCatalogs AS cata ON itemInvest.PosmCatalogId = cata.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	WHERE  
	posmInvest.CreationTime >= @FromDate 
	AND posmInvest.CreationTime <= @ToDate      
	AND itemInvest.Status = ISNULL(@Status, itemInvest.Status)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY posmInvest.CreationTime, customer.Code
END