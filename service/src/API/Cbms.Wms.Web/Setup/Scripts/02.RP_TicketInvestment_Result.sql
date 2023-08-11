IF OBJECT_ID(N'RP_TicketInvestment_Result') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_Result
END
GO
CREATE PROCEDURE RP_TicketInvestment_Result
@UserId int,
@FromDate DateTime,
@ToDate DateTime,
@StaffId int = NULL,
@LeadUserId int = NULL
AS

--DECLARE @UserId int --= 616
--DECLARE @FromDate DateTime = '2000-01-01'
--DECLARE @ToDate DateTime  = '2022-01-01'
--DECLARE @StaffId int = 579
--DECLARE @LeadUserId int --=
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
		, RewardItemCode = rewardItem.Code
		, RewardItemName = rewardItem.Name
		, ConsumerName = ticket.ConsumerName
		, ConsumerPhone = ticket.ConsumerPhone
		, TicketCode = ticket.Code
		, RewardQuantity = 1
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN TicketConsumerRewards AS consumerReward ON consumerReward.TicketInvestmentId = ticketInvest.Id
	INNER JOIN TicketConsumerRewardDetails AS consumerRewardDetail ON consumerRewardDetail.TicketConsumerRewardId = consumerReward.Id
	INNER JOIN Tickets AS ticket ON ticket.Id = consumerRewardDetail.TicketId
	INNER JOIN RewardItems AS rewardItem ON rewardItem.Id = consumerReward.RewardItemId
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
    AND CASE WHEN @LeadUserId IS NULL THEN teamLead.UserId ELSE @LeadUserId END = teamLead.UserId                              
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY ticketInvest.CreationTime, customer.Code, rewardItem.Code
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
		, RewardItemCode = rewardItem.Code
		, RewardItemName = rewardItem.Name
		, ConsumerName = ticket.ConsumerName
		, ConsumerPhone = ticket.ConsumerPhone
		, TicketCode = ticket.Code
		, RewardQuantity = 1
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN TicketConsumerRewards AS consumerReward ON consumerReward.TicketInvestmentId = ticketInvest.Id
	INNER JOIN TicketConsumerRewardDetails AS consumerRewardDetail ON consumerRewardDetail.TicketConsumerRewardId = consumerReward.Id
	INNER JOIN Tickets AS ticket ON ticket.Id = consumerRewardDetail.TicketId
	INNER JOIN RewardItems AS rewardItem ON rewardItem.Id = consumerReward.RewardItemId
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	WHERE  ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate
	AND CASE WHEN @LeadUserId IS NULL THEN teamLead.UserId ELSE @LeadUserId END = teamLead.UserId                
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY ticketInvest.CreationTime, customer.Code, rewardItem.Code
END

