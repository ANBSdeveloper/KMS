IF OBJECT_ID(N'RP_PosmInvestment_Request') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_PosmInvestment_Request
END

GO

CREATE PROC [dbo].[RP_PosmInvestment_Request]
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
						N'Rộng [' + FORMAT(itemInvest.Width, '#,###.00', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###.00', 'vi') + ']'
					WHEN item.CalcType = 2 THEN
						N'Cao [' + FORMAT(itemInvest.Height, '#,###.00', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###.00', 'vi') + ']'
					WHEN item.CalcType = 3 THEN
						N'Rộng [' + FORMAT(itemInvest.Width, '#,###.00', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###.00', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###.00', 'vi') + ']'
					WHEN item.CalcType = 4 THEN
						N'(Cạnh 1 [' + FORMAT(itemInvest.SideWidth1, '#,###.00', 'vi') + '] + ' + N'Cạnh 2 [' +  FORMAT(itemInvest.SideWidth2, '#,###.00', 'vi') + ']) x ' + N'Mặt rộng [' +  FORMAT(itemInvest.Width, '#,###.00', 'vi') + ']'
					ELSE ''
				 END
		 , StaffPhone = staffUser.PhoneNumber
		, SurveyPhotoLink1 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/0'
		, SurveyPhotoLink2 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/1'
		, SurveyPhotoLink3 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/2'
		, SurveyPhotoLink4 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/3'
		, Status = CASE 
				WHEN itemInvest.Status = 10 THEN     N'Đề xuất'
				WHEN itemInvest.Status = 20 THEN  N'ASM từ chốt duyệt đề xuất'
				WHEN itemInvest.Status = 30 THEN  N'ASM duyệt đề xuất'
				WHEN itemInvest.Status = 40 THEN  N'RSM từ chối duyệt đề xuất'
				WHEN itemInvest.Status = 50 THEN  N'RSM duyệt đề xuất'
				WHEN itemInvest.Status = 60 THEN  N'Trade từ chốt PYC'
				WHEN itemInvest.Status = 70 THEN N'Trade xác nhận PYC hợp lệ'
				WHEN itemInvest.Status = 80 THEN  N'Giám đốc từ chối duyệt'
				WHEN itemInvest.Status = 90 THEN  N'Giám đốc duyệt PYC'
				WHEN itemInvest.Status = 100 THEN   N'Đơn hàng không hợp lệ'
				WHEN itemInvest.Status = 110 THEN  N'SS đề xuất bổ sung chi phí'
				WHEN itemInvest.Status = 120 THEN  N'ASM xác nhận bổ sung'
				WHEN itemInvest.Status = 130 THEN  N'RSM xác nhận bổ sung'
				WHEN itemInvest.Status = 140 THEN  N'Trade xác nhận bổ sung'
				WHEN itemInvest.Status = 150 THEN N'Đơn hàng hợp lệ'
				WHEN itemInvest.Status = 160 THEN  N'Xác nhận sản xuất 1 (MKT)'
				WHEN itemInvest.Status = 170 THEN  N'Xác nhận sản xuất 2 (SS)'
				WHEN itemInvest.Status = 180 THEN  N'Xác nhận cho NCC sản xuất'
				WHEN itemInvest.Status = 190 THEN  N'Nghiệm thu (SS)'
				WHEN itemInvest.Status = 200 THEN  N'Xác nhận nghiệm thu 1(ASM)'
				WHEN itemInvest.Status = 210 THEN N'Xác nhận nghiệm thu 2(Trade)'
				ELSE 'NA'
		END
	FROM PosmInvestments AS posmInvest
	INNER JOIN PosmInvestmentItems AS itemInvest ON posmInvest.Id = itemInvest.PosmInvestmentId
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN CustomerLocations AS cl ON posmInvest.CustomerLocationId = cl.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN PosmItems AS item ON itemInvest.PosmItemId = item.Id
	INNER JOIN PosmClasses AS clazz ON itemInvest.PosmClassId = clazz.Id
	INNER JOIN PosmCatalogs AS cata ON itemInvest.PosmCatalogId = cata.Id
	INNER JOIN Staffs AS staff ON posmInvest.RegisterStaffId = staff.Id
	INNER JOIN Users AS staffUser ON staff.UserId = staffUser.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	WHERE  posmInvest.CreationTime >= @FromDate 
	AND posmInvest.CreationTime <= @ToDate       
	--AND itemInvest.Status >= 90
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
					N'Rộng [' + FORMAT(itemInvest.Width, '#,###.00', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###.00', 'vi') + ']'
				WHEN item.CalcType = 2 THEN
					N'Cao [' + FORMAT(itemInvest.Height, '#,###.00', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###.00', 'vi') + ']'
				WHEN item.CalcType = 3 THEN
					N'Rộng [' + FORMAT(itemInvest.Width, '#,###.00', 'vi') + '] x ' + N'Cao [' +  FORMAT(itemInvest.Height, '#,###.00', 'vi') + '] x ' + N'Sâu [' +  FORMAT(itemInvest.Depth, '#,###.00', 'vi') + ']'
				WHEN item.CalcType = 4 THEN
					N'(Cạnh 1 [' + FORMAT(itemInvest.SideWidth1, '#,###.00', 'vi') + '] + ' + N'Cạnh 2 [' +  FORMAT(itemInvest.SideWidth2, '#,###.00', 'vi') + ']) x ' + N'Mặt rộng [' +  FORMAT(itemInvest.Width, '#,###.00', 'vi') + ']'
				ELSE ''
				 END 
		, StaffPhone = staffUser.PhoneNumber
		, SurveyPhotoLink1 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/0'
		, SurveyPhotoLink2 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/1'
		, SurveyPhotoLink3 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/2'
		, SurveyPhotoLink4 ='https://api-kms.vitadairy.vn/api/v1/posm-investments/items/' + CAST(itemInvest.Id AS VARCHAR) + '/survey-images/3'
		, Status = CASE 
			    WHEN itemInvest.Status = 10 THEN     N'Đề xuất'
				WHEN itemInvest.Status = 20 THEN  N'ASM từ chốt duyệt đề xuất'
				WHEN itemInvest.Status = 30 THEN  N'ASM duyệt đề xuất'
				WHEN itemInvest.Status = 40 THEN  N'RSM từ chối duyệt đề xuất'
				WHEN itemInvest.Status = 50 THEN  N'RSM duyệt đề xuất'
				WHEN itemInvest.Status = 60 THEN  N'Trade từ chốt PYC'
				WHEN itemInvest.Status = 70 THEN N'Trade xác nhận PYC hợp lệ'
				WHEN itemInvest.Status = 80 THEN  N'Giám đốc từ chối duyệt'
				WHEN itemInvest.Status = 90 THEN  N'Giám đốc duyệt PYC'
				WHEN itemInvest.Status = 100 THEN   N'Đơn hàng không hợp lệ'
				WHEN itemInvest.Status = 110 THEN  N'SS đề xuất bổ sung chi phí'
				WHEN itemInvest.Status = 120 THEN  N'ASM xác nhận bổ sung'
				WHEN itemInvest.Status = 130 THEN  N'RSM xác nhận bổ sung'
				WHEN itemInvest.Status = 140 THEN  N'Trade xác nhận bổ sung'
				WHEN itemInvest.Status = 150 THEN N'Đơn hàng hợp lệ'
				WHEN itemInvest.Status = 160 THEN  N'Xác nhận sản xuất 1 (MKT)'
				WHEN itemInvest.Status = 170 THEN  N'Xác nhận sản xuất 2 (SS)'
				WHEN itemInvest.Status = 180 THEN  N'Xác nhận cho NCC sản xuất'
				WHEN itemInvest.Status = 190 THEN  N'Nghiệm thu (SS)'
				WHEN itemInvest.Status = 200 THEN  N'Xác nhận nghiệm thu 1(ASM)'
				WHEN itemInvest.Status = 210 THEN N'Xác nhận nghiệm thu 2(Trade)'
				ELSE 'NA'
		END
	FROM PosmInvestments AS posmInvest
	INNER JOIN PosmInvestmentItems AS itemInvest ON posmInvest.Id = itemInvest.PosmInvestmentId
	INNER JOIN CustomerLocations AS cl ON posmInvest.CustomerLocationId = cl.Id
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN PosmItems AS item ON itemInvest.PosmItemId = item.Id
	INNER JOIN PosmClasses AS clazz ON itemInvest.PosmClassId = clazz.Id
	INNER JOIN PosmCatalogs AS cata ON itemInvest.PosmCatalogId = cata.Id
	INNER JOIN Staffs AS staff ON posmInvest.RegisterStaffId = staff.Id
	INNER JOIN Users AS staffUser ON staff.UserId = staffUser.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	WHERE  
	posmInvest.CreationTime >= @FromDate 
	AND posmInvest.CreationTime <= @ToDate      
	--AND itemInvest.Status >= 90
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY posmInvest.CreationTime, customer.Code
END