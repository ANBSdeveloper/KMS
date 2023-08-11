IF OBJECT_ID(N'RP_Analytic') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_Analytic
END

GO

CREATE PROC RP_Analytic
@UserId INT,
@CycleId INT
AS


DECLARE @StaffId INT
DECLARE @FromDate DATE 
DECLARE @ToDate DATE
DECLARE @Shops INT, @KeyShops INT, @TicketShops INT, @InvestAmount DECIMAL, 
		@RewardAmount DECIMAL, @MaterialAmount DECIMAL, @AvgInvestmentAmount DECIMAL, 
		@Orders INT, @Items INT, @RewardQuantity INT, @CommitmentAmount INT, @ActualAmount INT,
		@DoingTickets INT, @CompletedTickets INT

SELECT @FromDate = FromDate, @ToDate = ToDate FROM Cycles WHERE Id = @CycleId

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
		  @Shops = ISNULL(COUNT(*), 0)
		, @KeyShops = ISNULL(COUNT(CASE  customer.IsKeyShop WHEN 1 Then 1 ELSE null END),0)
	FROM Customers AS customer
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE   EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId);

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
		  @TicketShops = ISNULL(COUNT(*), 0)
		, @InvestAmount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
		, @RewardAmount = ISNULL(SUM(ticketInvest.RewardAmount), 0)
		, @MaterialAmount = ISNULL(SUM(ticketInvest.MaterialAmount), 0)
		, @AvgInvestmentAmount = CASE ISNULL(COUNT(*), 0) WHEN 0 THEN 0 ELSE ISNULL(SUM(ticketInvest.InvestmentAmount), 0) / ISNULL(COUNT(*), 1) END
		, @Orders = ISNULL(SUM([orderAgg].Orders), 0)
		, @Items = ISNULL(SUM([orderAgg].Items), 0)
		, @RewardQuantity = ISNULL(SUM([rewardAgg].RewardQuantity), 0)
		, @CommitmentAmount = ISNULL(SUM(ticketInvest.CommitmentAmount), 0)
		, @ActualAmount = ISNULL(SUM(ISNULL(ticketInvest.CommitmentAmount,0)), 0)
		, @DoingTickets = ISNULL(COUNT(CASE WHEN ticketInvest.Status IN (10, 30, 40, 60, 80, 100, 120,130,140,150,160) THEN 1 ELSE NULL END), 0)
		, @CompletedTickets = ISNULL(COUNT(CASE WHEN ticketInvest.Status IN (170) THEN 1 ELSE NULL END), 0)
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	OUTER APPLY (
		SELECT Orders = COUNT(*), Items = SUM([order].TotalQuantity) FROM Orders AS [order]
		WHERE [order].TicketInvestmentId = ticketInvest.Id
	) AS [orderAgg]
	OUTER APPLY (
		SELECT RewardQuantity = COUNT(*) FROM TicketConsumerRewardDetails AS detail
		INNER JOIN TicketConsumerRewards AS consumer ON consumer.Id = detail.TicketConsumerRewardId
		WHERE consumer.TicketInvestmentId = ticketInvest.Id
	) AS [rewardAgg]
	WHERE 
	  CAST(ticketInvest.CreationTime AS DATE) >= CAST(@FromDate AS DATE) 
	AND CAST(ticketInvest.CreationTime AS DATE) <= CAST(@ToDate AS DATE)  
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	AND EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)

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
		   @Shops = ISNULL(COUNT(*), 0)
		, @KeyShops = ISNULL(COUNT(CASE  customer.IsKeyShop WHEN 1 Then 1 ELSE null END),0)
	FROM Customers AS customer
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE   EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	);

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
		  @TicketShops = ISNULL(COUNT(*), 0)
		, @InvestAmount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
		, @RewardAmount = ISNULL(SUM(ticketInvest.RewardAmount), 0)
		, @MaterialAmount = ISNULL(SUM(ticketInvest.MaterialAmount), 0)
		, @AvgInvestmentAmount = CASE ISNULL(COUNT(*), 0) WHEN 0 THEN 0 ELSE ISNULL(SUM(ticketInvest.InvestmentAmount), 0) / ISNULL(COUNT(*), 1) END
		, @Orders = ISNULL(SUM([orderAgg].Orders), 0)
		, @Items = ISNULL(SUM([orderAgg].Items), 0)
		, @RewardQuantity = ISNULL(SUM([rewardAgg].RewardQuantity), 0)
		, @CommitmentAmount = ISNULL(SUM(ticketInvest.CommitmentAmount), 0)
		, @ActualAmount = ISNULL(SUM(ISNULL(ticketInvest.CommitmentAmount,0)), 0)
		, @DoingTickets = ISNULL(COUNT(CASE WHEN ticketInvest.Status IN (10, 30, 40, 60, 80, 100, 120,130,140,150,160) THEN 1 ELSE NULL END), 0)
		, @CompletedTickets = ISNULL(COUNT(CASE WHEN ticketInvest.Status IN (170) THEN 1 ELSE NULL END), 0)
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	OUTER APPLY (
		SELECT Orders = COUNT(*), Items = SUM([order].TotalQuantity) FROM Orders AS [order]
		WHERE [order].TicketInvestmentId = ticketInvest.Id
	) AS [orderAgg]
	OUTER APPLY (
		SELECT RewardQuantity = COUNT(*) FROM TicketConsumerRewardDetails AS detail
		INNER JOIN TicketConsumerRewards AS consumer ON consumer.Id = detail.TicketConsumerRewardId
		WHERE consumer.TicketInvestmentId = ticketInvest.Id
	) AS [rewardAgg]
	WHERE 
	  CAST(ticketInvest.CreationTime AS DATE) >= CAST(@FromDate AS DATE) 
	AND CAST(ticketInvest.CreationTime AS DATE) <= CAST(@ToDate AS DATE)  
	AND EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)

END

SELECT 
	Shops = @Shops, 
	KeyShops = @KeyShops, 
	TicketShops = @TicketShops, 
	InvestAmount = @InvestAmount, 
	RewardAmount = @RewardAmount,
	MaterialAmount = @MaterialAmount,
	AvgInvestmentAmount = @AvgInvestmentAmount,
	Orders = @Orders,
	Items = @Items,
	RewardQuantity = @RewardQuantity,
	CommitmentAmount = @CommitmentAmount,
	ActualAmount = @ActualAmount,
	DoingTickets = @DoingTickets,
	CompletedTickets = @CompletedTickets
