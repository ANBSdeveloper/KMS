IF OBJECT_ID(N'RP_TicketInvestment_Remark') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_Remark
END

GO

CREATE PROC RP_TicketInvestment_Remark -- RP_TicketInvestment_Remark 1, '2020-01-01', '2022-01-01', NULL, NULL
@UserId int,
@FromDate DateTime,
@ToDate DateTime,
@StaffId int = NULL,
@LeadUserId int = NULL
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
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, CreationTime = FORMAT(ticketInvest.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
		, IssueBeginDate = FORMAT(ticketInvest.IssueTicketBeginDate,'yyyy-MM-ddTHH:mm:ss') 
		, IssueEndDate = FORMAT(ticketInvest.IssueTicketEndDate,'yyyy-MM-ddTHH:mm:ss') 
		, OperationDate = FORMAT(ticketInvest.OperationDate,'yyyy-MM-ddTHH:mm:ss') 
		, InvestmentCode = ticketInvest.Code
		, TicketQuantity = ticketInvest.TicketQuantity
		, UsedTicketQuantity = ticketInvest.SmsTicketQuantity
		, SalesPlanAmount = ticketInvest.SalesPlanAmount
		, ActualSalesAmount = ticketInvest.ActualSalesAmount
		, InvestmentAmount = ticketInvest.InvestmentAmount
		, CommitmentAmount = ticketInvest.CommitmentAmount
		, StockQuantity = ticketInvest.StockQuantity
		, StockQuantityAfter = ISNULL(operation.StockQuantity, 0)
		, TicketValue = CASE WHEN TicketQuantity = 0 THEN 0 ELSE ticketInvest.SalesPlanAmount / ticketInvest.TicketQuantity END
		, ActualInvestmentAmount = reward.ActualInvestmentAmount
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	LEFT JOIN TicketOperations AS operation ON operation.TicketInvestmentId = ticketInvest.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	OUTER APPLY (
		SELECT ActualInvestmentAmount = ISNULL(SUM(consumerReward.RewardQuantity * rewardItem.Price), 0) 
		FROM  TicketConsumerRewards AS consumerReward 
		INNER JOIN TicketRewardItems AS rewardItem ON rewardItem.TicketInvestmentId = consumerReward.TicketInvestmentId AND consumerReward.RewardItemId = rewardItem.RewardItemId
		WHERE consumerReward.TicketInvestmentId = ticketInvest.Id
	) AS reward
	WHERE  
	ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate                     
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	AND CASE WHEN @LeadUserId IS NULL THEN teamLead.UserId ELSE @LeadUserId END = teamLead.UserId    
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	ORDER BY ticketInvest.CreationTime, customer.Code
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
		, LeadCode = teamLead.UserName
		, LeadName = teamLead.Name
		, ShopCode = customer.Code
		, ShopName = customer.Name
		, ContactName = customer.ContactName
		, Phone = customer.MobilePhone
		, Address = customer.Address
		, CreationTime = FORMAT(ticketInvest.CreationTime,'yyyy-MM-ddTHH:mm:ss') 
		, IssueBeginDate = FORMAT(ticketInvest.IssueTicketBeginDate,'yyyy-MM-ddTHH:mm:ss') 
		, IssueEndDate = FORMAT(ticketInvest.IssueTicketEndDate,'yyyy-MM-ddTHH:mm:ss') 
		, OperationDate = FORMAT(ticketInvest.OperationDate,'yyyy-MM-ddTHH:mm:ss') 
		, InvestmentCode = ticketInvest.Code
		, TicketQuantity = ticketInvest.TicketQuantity
		, UsedTicketQuantity = ticketInvest.SmsTicketQuantity
		, SalesPlanAmount = ticketInvest.SalesPlanAmount
		, ActualSalesAmount = ticketInvest.ActualSalesAmount
		, InvestmentAmount = ticketInvest.InvestmentAmount
		, CommitmentAmount = ticketInvest.CommitmentAmount
		, StockQuantity = ticketInvest.StockQuantity
		, StockQuantityAfter = ISNULL(operation.StockQuantity, 0)
		, TicketValue = CASE WHEN TicketQuantity = 0 THEN 0 ELSE ticketInvest.SalesPlanAmount / ticketInvest.TicketQuantity END
		, ActualInvestmentAmount = reward.ActualInvestmentAmount
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	LEFT JOIN TicketOperations AS operation ON operation.TicketInvestmentId = ticketInvest.Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	OUTER APPLY (
		SELECT ActualInvestmentAmount = ISNULL(SUM(consumerReward.RewardQuantity * rewardItem.Price), 0) 
		FROM  TicketConsumerRewards AS consumerReward 
		INNER JOIN TicketRewardItems AS rewardItem ON rewardItem.TicketInvestmentId = consumerReward.TicketInvestmentId AND consumerReward.RewardItemId = rewardItem.RewardItemId
		WHERE consumerReward.TicketInvestmentId = ticketInvest.Id
	) AS reward
	WHERE  
	ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate                     
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
    AND CASE WHEN @LeadUserId IS NULL THEN teamLead.UserId ELSE @LeadUserId END = teamLead.UserId    
	ORDER BY ticketInvest.CreationTime, customer.Code
END