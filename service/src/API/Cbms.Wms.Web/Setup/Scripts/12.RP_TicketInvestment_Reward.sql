IF OBJECT_ID(N'RP_TicketInvestment_Reward') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_Reward
END
GO
CREATE PROCEDURE [dbo].[RP_TicketInvestment_Reward] --- RP_TicketInvestment_Reward 1,'2000-01-01','2022-01-01',579
@UserId int,
@FromDate DateTime,
@ToDate DateTime,
@StaffId int = NULL
AS

--DECLARE @UserId int --= 616
--DECLARE @FromDate DateTime = '2000-01-01'
--DECLARE @ToDate DateTime  = '2022-01-01'
--DECLARE @StaffId int = 579

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
	SELECT * INTO #tbTam
	FROM (
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
		, SSMName = staffSS.Name
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
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
		, RewardItemCode = rewardItem.Code
		, RewardItemName = rewardItem.Name
		, QuantityRegistrations = ticketRewardItem.Quantity --- Số lượng đăng ký
		, IsReceivedRewardItem = ISNULL(b.IsReceived, CAST(0 AS BIT)) --- Đã Nhận Quà
		, IsSentDesign = CAST(0 AS BIT) --- Đã gửi Thiết Kế
		, IsReceivedMaterial = CAST(0 AS BIT) --- Đã chuẩn bị Vật tư
		, DateOfReceipt = FORMAT(b.CreationTime,'dd/MM/yyyy') 
		, Type =N'Quà Tặng' -- Loại 

		
	FROM TicketInvestments AS ticketInvest
		INNER JOIN TicketRewardItems AS ticketRewardItem ON ticketInvest.Id = ticketRewardItem.TicketInvestmentId
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
		INNER JOIN RewardItems AS rewardItem ON ticketRewardItem.RewardItemId = rewardItem.Id		
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
		OUTER APPLY (
			SELECT TOP 1 ticketProgressRewardItem.IsReceived,ticketProgressRewardItem.CreationTime  FROM TicketProgressRewardItems AS ticketProgressRewardItem 
			INNER JOIN TicketProgresses AS ticketProgresse ON ticketProgressRewardItem.TicketProgressId = ticketProgresse.Id
			WHERE ticketProgresse.TicketInvestmentId = ticketInvest.Id AND ticketProgressRewardItem.RewardItemId = ticketRewardItem.RewardItemId
			ORDER BY ticketProgressRewardItem.CreationTime DESC
		) AS b
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate     
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	--ORDER BY ticketInvest.CreationTime, customer.Code, rewardItem.Code

	UNION ALL

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
		, SSMName = staffSS.Name
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
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
		, RewardItemCode = material.Code
		, RewardItemName = material.Name
		, QuantityRegistrations = ticketMaterial.RegisterQuantity --- Số lượng đăng ký
		, IsReceivedRewardItem = CAST(0 AS BIT) --- Đã Nhận Quà
		, IsSentDesign = ISNULL(b.IsSentDesign,CAST(0 AS BIT)) --- Đã gửi Thiết Kế
		, IsReceivedMaterial = ISNULL(b.IsReceived, CAST(0 AS BIT))  --- Đã chuẩn bị Vật tư
		, DateOfReceipt = FORMAT(b.CreationTime,'dd/MM/yyyy') 
		, Type =N'Vật Tư' -- Loại 

	FROM TicketInvestments AS ticketInvest
		INNER JOIN TicketMaterials AS ticketMaterial ON ticketInvest.Id = ticketMaterial.TicketInvestmentId
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
		INNER JOIN Materials AS material ON ticketMaterial.MaterialId = material.Id		
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
		OUTER APPLY (
			SELECT TOP 1 ticketProgressMaterial.IsReceived,ticketProgressMaterial.CreationTime, ticketProgressMaterial.IsSentDesign  FROM TicketProgressMaterials AS ticketProgressMaterial 
			INNER JOIN TicketProgresses AS ticketProgresse ON ticketProgressMaterial.TicketProgressId = ticketProgresse.Id
			WHERE ticketProgresse.TicketInvestmentId = ticketInvest.Id AND ticketProgressMaterial.MaterialId = material.MaterialTypeId
			ORDER BY ticketProgressMaterial.CreationTime DESC
		) AS b
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate     
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)

	) AS A


	SELECT * FROM #tbTam ORDER BY TicketInvestmentCode , RewardItemCode

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


	SELECT * INTO #tbTam1
	FROM (
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
		, SSMName = staffSS.Name
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
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
		, RewardItemCode = rewardItem.Code
		, RewardItemName = rewardItem.Name
		, QuantityRegistrations = ticketRewardItem.Quantity --- Số lượng đăng ký
		, IsReceivedRewardItem = ISNULL(b.IsReceived, CAST(0 AS BIT)) --- Đã Nhận Quà
		, IsSentDesign = CAST(0 AS BIT) --- Đã gửi Thiết Kế
		, IsReceivedMaterial = CAST(0 AS BIT) --- Đã chuẩn bị Vật tư
		, DateOfReceipt = FORMAT(b.CreationTime,'dd/MM/yyyy') 
		, Type =N'Quà Tặng' -- Loại 

		
	FROM TicketInvestments AS ticketInvest
		INNER JOIN TicketRewardItems AS ticketRewardItem ON ticketInvest.Id = ticketRewardItem.TicketInvestmentId
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
		INNER JOIN RewardItems AS rewardItem ON ticketRewardItem.RewardItemId = rewardItem.Id		
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
		OUTER APPLY (
			SELECT TOP 1 ticketProgressRewardItem.IsReceived,ticketProgressRewardItem.CreationTime  FROM TicketProgressRewardItems AS ticketProgressRewardItem 
			INNER JOIN TicketProgresses AS ticketProgresse ON ticketProgressRewardItem.TicketProgressId = ticketProgresse.Id
			WHERE ticketProgresse.TicketInvestmentId = ticketInvest.Id AND ticketProgressRewardItem.RewardItemId = ticketRewardItem.RewardItemId
			ORDER BY ticketProgressRewardItem.CreationTime DESC
		) AS b
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate     
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	--ORDER BY ticketInvest.CreationTime, customer.Code, rewardItem.Code

	UNION ALL

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
		, SSMName = staffSS.Name
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
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
		, RewardItemCode = material.Code
		, RewardItemName = material.Name
		, QuantityRegistrations = ticketMaterial.RegisterQuantity --- Số lượng đăng ký
		, IsReceivedRewardItem = CAST(0 AS BIT) --- Đã Nhận Quà
		, IsSentDesign = ISNULL(b.IsSentDesign,CAST(0 AS BIT)) --- Đã gửi Thiết Kế
		, IsReceivedMaterial = ISNULL(b.IsReceived, CAST(0 AS BIT))  --- Đã chuẩn bị Vật tư
		, DateOfReceipt = FORMAT(b.CreationTime,'dd/MM/yyyy') 
		, Type =N'Vật Tư' -- Loại 

	FROM TicketInvestments AS ticketInvest
		INNER JOIN TicketMaterials AS ticketMaterial ON ticketInvest.Id = ticketMaterial.TicketInvestmentId
		INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
		INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
		INNER JOIN Materials AS material ON ticketMaterial.MaterialId = material.Id		
		LEFT JOIN Staffs AS staffRSM ON customer.RsmStaffId = staffRSM.Id
		LEFT JOIN Staffs AS staffASM ON customer.AsmStaffId = staffASM.Id
		LEFT JOIN Staffs AS staffSS ON customer.SalesSupervisorStaffId = staffSS.Id
		LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
		LEFT JOIN Areas AS area ON area.Id = branch.AreaId
		LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
		CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
		OUTER APPLY (
			SELECT TOP 1 ticketProgressMaterial.IsReceived,ticketProgressMaterial.CreationTime, ticketProgressMaterial.IsSentDesign  FROM TicketProgressMaterials AS ticketProgressMaterial 
			INNER JOIN TicketProgresses AS ticketProgresse ON ticketProgressMaterial.TicketProgressId = ticketProgresse.Id
			WHERE ticketProgresse.TicketInvestmentId = ticketInvest.Id AND ticketProgressMaterial.MaterialId = material.MaterialTypeId
			ORDER BY ticketProgressMaterial.CreationTime DESC
		) AS b
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate       
	AND ticketInvest.Status IN (120, 140, 150, 160, 170)
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)

	) AS A

	SELECT * FROM #tbTam1 ORDER BY TicketInvestmentCode , RewardItemCode

END