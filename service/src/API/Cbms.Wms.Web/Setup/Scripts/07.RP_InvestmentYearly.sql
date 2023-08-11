IF OBJECT_ID(N'RP_InvestmentYearly') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_InvestmentYearly
END

GO

CREATE PROC RP_InvestmentYearly 
@UserId INT
AS


DECLARE @StaffId INT

DECLARE @YearMonth AS Table(YearMonth VARCHAR(10), SalesAmount DECIMAL, TicketInvestAmount DECIMAL, PosmInvestAmount DECIMAL)
DECLARE @TicketInvest AS Table(YearMonth VARCHAR(10), Amount DECIMAL)
DECLARE @PosmInvest AS Table(YearMonth VARCHAR(10), Amount DECIMAL)

DECLARE @StartDate DATETIME = DATEADD(month, -12, DATEADD(m, DATEDIFF(m, 0, GETDATE()), 0))
DECLARE @EndDate DATETIME = FORMAT(EOMONTH(GETDATE()), 'yyyy-MM-dd') + ' 23:59:59';

INSERT INTO @YearMonth (YearMonth, SalesAmount, TicketInvestAmount, PosmInvestAmount)
VALUES 
(FORMAT(GETDATE(), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -1, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -2, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -3, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -4, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -5, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -6, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -7, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -8, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -9, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -10, GETDATE()), 'yyyy-MM'), 0,0, 0)
,(FORMAT(DATEADD(MONTH, -11, GETDATE()), 'yyyy-MM'), 0,0, 0)


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
	INSERT INTO @TicketInvest
	SELECT 
		  YearMonth =  FORMAT(ticketInvest.CreationTime, 'yyyy-MM')
		, Amount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE 
	ticketInvest.CreationTime BETWEEN @StartDate AND @EndDate AND
	EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	GROUP BY FORMAT(ticketInvest.CreationTime, 'yyyy-MM');

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
	INSERT INTO @PosmInvest
	SELECT 
		  YearMonth =  FORMAT(posmInvest.CreationTime, 'yyyy-MM')
		, Amount = ISNULL(SUM(posmInvest.InvestmentAmount), 0)
	FROM PosmInvestments AS posmInvest
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE 
	posmInvest.CreationTime BETWEEN @StartDate AND @EndDate AND
	 EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	AND (customer.SalesSupervisorStaffId = @StaffId OR customer.AsmStaffId = @StaffId OR customer.RsmStaffId = @StaffId)
	GROUP BY FORMAT(posmInvest.CreationTime, 'yyyy-MM')


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
	INSERT INTO @TicketInvest
	SELECT 
		  YearMonth =  FORMAT(ticketInvest.CreationTime, 'yyyy-MM')
		, Amount = ISNULL(SUM(ticketInvest.InvestmentAmount), 0)
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE 
	ticketInvest.CreationTime BETWEEN @StartDate AND @EndDate AND
	 EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	GROUP BY FORMAT(ticketInvest.CreationTime, 'yyyy-MM');

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
	INSERT INTO @PosmInvest
	SELECT 
		  YearMonth =  FORMAT(posmInvest.CreationTime, 'yyyy-MM')
		, Amount = ISNULL(SUM(posmInvest.InvestmentAmount), 0)
	FROM PosmInvestments AS posmInvest
	INNER JOIN Customers AS customer ON posmInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	WHERE 
	posmInvest.CreationTime BETWEEN @StartDate AND @EndDate AND
	 EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId
	)
	GROUP BY FORMAT(posmInvest.CreationTime, 'yyyy-MM')
END

SELECT 
	  YearMonth = yearMonth.YearMonth
	, TicketAmount = ISNULL(ticketInvest.Amount, 0)
	, PosmAmount = ISNULL(posmInvest.Amount, 0)
	, SalesAmount = ISNULL(sales.Amount, 0)
FROM @YearMonth AS yearMonth
LEFT JOIN @TicketInvest AS ticketInvest ON  yearMonth.YearMonth = ticketInvest.YearMonth
LEFT JOIN @PosmInvest AS posmInvest ON  yearMonth.YearMonth = posmInvest.YearMonth
LEFT JOIN CustomerSales AS sales ON yearMonth.YearMonth = sales.YearMonth
WHERE ticketInvest.Amount IS NOT NULL OR posmInvest.Amount IS NOT NULL OR sales.Amount IS NOT NULL
ORDER BY YearMonth

