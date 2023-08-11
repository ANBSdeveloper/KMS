IF OBJECT_ID(N'RP_Analytic6Months') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_Analytic6Months
END

GO

CREATE PROC RP_Analytic6Months
@UserId INT
AS


DECLARE @StaffId INT

DECLARE @YearMonth AS Table(YearMonth VARCHAR(10), InvestmentAmount DECIMAL, ActualAmount DECIMAL, Orders INT, RewardQuantity INT)
DECLARE @Result AS Table(YearMonth VARCHAR(10), InvestmentAmount DECIMAL, ActualAmount DECIMAL, Orders INT, RewardQuantity INT)


INSERT INTO @YearMonth (YearMonth, InvestmentAmount, ActualAmount)
VALUES 
(FORMAT(GETDATE(), 'yyyy-MM'), 0,0)
,(FORMAT(DATEADD(MONTH, -1, GETDATE()), 'yyyy-MM'), 0,0)
,(FORMAT(DATEADD(MONTH, -2, GETDATE()), 'yyyy-MM'), 0,0)
,(FORMAT(DATEADD(MONTH, -3, GETDATE()), 'yyyy-MM'), 0,0)
,(FORMAT(DATEADD(MONTH, -4, GETDATE()), 'yyyy-MM'), 0,0)
,(FORMAT(DATEADD(MONTH, -5, GETDATE()), 'yyyy-MM'), 0,0)

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
	INSERT INTO @Result
	SELECT 
		  YearMonth =  FORMAT(ticketInvest.CreationTime, 'yyyy-MM')
		, InvestmentAmount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
		, ActualAmount = ISNULL(SUM(ISNULL(ticketInvest.CommitmentAmount,0)), 0)
		, Orders = ISNULL(SUM([orderAgg].Orders), 0)
		, RewardQuantity = ISNULL(SUM([rewardAgg].RewardQuantity), 0)
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
	 EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	GROUP BY FORMAT(ticketInvest.CreationTime, 'yyyy-MM')

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
	INSERT INTO @Result
	SELECT 
		  YearMonth =  FORMAT(ticketInvest.CreationTime, 'yyyy-MM')
		, InvestmentAmount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
		, ActualAmount = ISNULL(SUM(ISNULL(ticketInvest.CommitmentAmount,0)), 0)
		, Orders = ISNULL(SUM([orderAgg].Orders), 0)
		, RewardQuantity = ISNULL(SUM([rewardAgg].RewardQuantity), 0)
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
	 EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	GROUP BY FORMAT(ticketInvest.CreationTime, 'yyyy-MM')
END

SELECT 
	  YearMonth = yearMonth.YearMonth
	, InvestmentAmount = ISNULL(result.InvestmentAmount, 0)
	, ActualAmount = ISNULL(result.ActualAmount, 0)
	, Orders = ISNULL(result.Orders, 0)
	, RewardQuantity = ISNULL(result.RewardQuantity, 0)
FROM @YearMonth AS yearMonth
LEFT JOIN @Result AS result ON  yearMonth.YearMonth = result.YearMonth
ORDER BY YearMonth
