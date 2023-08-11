IF OBJECT_ID(N'RP_Budget') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_Budget
END

GO


CREATE PROC [dbo].[RP_Budget]
@UserId INT,
@CycleId INT
AS


DECLARE @StaffId AS INT
DECLARE @ZoneId AS INT
DECLARE @AreaId AS INT
DECLARE @StaffType AS VARCHAR(10)
SELECT @StaffId = Id, @AreaId = AreaId, @ZoneId = ZoneId, @StaffType = StaffTypeCode FROM Staffs WHERE UserId = @UserId

IF @StaffId IS NOT NULL
BEGIN
	IF @StaffType = 'RSM'
	BEGIN
		SELECT 
			 AllocateAmount = ISNULL(SUM(budgetDetail.AllocateAmount), 0)
		   , UsedAmount = ISNULL(SUM(budgetDetail.UsedAmount), 0)
		   , RemainAmount = ISNULL(SUM(budgetDetail.RemainAmount), 0)
		FROM BudgetZones AS budgetDetail 
		INNER JOIN Budgets AS budget ON budgetDetail.BudgetId = budget.Id
		WHERE budget.CycleId = @CycleId AND budgetDetail.ZoneId = @ZoneId
	END
	ELSE IF @StaffType = 'ASM'
	BEGIN
		SELECT 
			 AllocateAmount = ISNULL(SUM(budgetDetail.AllocateAmount), 0)
		   , UsedAmount = ISNULL(SUM(budgetDetail.UsedAmount), 0)
		   , RemainAmount = ISNULL(SUM(budgetDetail.RemainAmount), 0)
		FROM BudgetAreas AS budgetDetail 
		INNER JOIN Budgets AS budget ON budgetDetail.BudgetId = budget.Id
		WHERE budget.CycleId = @CycleId AND budgetDetail.AreaId = @AreaId
	END
	ELSE IF @StaffType = 'SS'
	BEGIN
		SELECT 
			 AllocateAmount = ISNULL(SUM(budgetDetail.AllocateAmount), 0)
		   , UsedAmount = ISNULL(SUM(budgetDetail.UsedAmount), 0)
		   , RemainAmount = ISNULL(SUM(budgetDetail.RemainAmount), 0)
		FROM BudgetBranches AS budgetDetail 
		INNER JOIN Budgets AS budget ON budgetDetail.BudgetId = budget.Id
		WHERE budget.CycleId = @CycleId 
		AND budgetDetail.BranchId IN(
			SELECT DISTINCT BranchId 
			FROM Customers 
			WHERE SalesSupervisorStaffId = @StaffId
		)
	END
	ELSE 
	BEGIN
		SELECT 
			 AllocateAmount = 0
		   , UsedAmount = 0
		   , RemainAmount = 0
	END
END
ELSE
BEGIN
	SELECT 
		  AllocateAmount = ISNULL(SUM(budgetDetail.AllocateAmount), 0)
		, UsedAmount = ISNULL(SUM(budgetDetail.UsedAmount), 0)
		, RemainAmount = ISNULL(SUM(budgetDetail.RemainAmount), 0)
	FROM BudgetZones AS budgetDetail
	INNER JOIN Budgets AS budget ON budgetDetail.BudgetId = budget.Id
	WHERE budget.CycleId = @CycleId
END