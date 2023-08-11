IF OBJECT_ID(N'RP_PosmInvestment_Budget') IS NOT NULL
BEGIN
    DROP PROCEDURE [RP_PosmInvestment_Budget]
END

GO

CREATE PROC [dbo].[RP_PosmInvestment_Budget]
@UserId int,
@CycleId INT,
@ZoneId INT
AS

SELECT 
 b.InvestmentType
, ZoneName = z.Name
, ZoneAllocateAmount = bz.AllocateAmount
, ZoneUsedAmount = bz.UsedAmount
, ZoneRemainAmount = bz.RemainAmount
, ZoneTempRemainAmount =  bz.TempRemainAmount
, ZoneTempUsedAmount = bz.TempUsedAmount
, AreaName = a.Name
, AreaAllocateAmount = ba.AllocateAmount
, AreaUsedAmount = ba.UsedAmount
, AreaRemainAmount = ba.RemainAmount
, AreaTempRemainAmount =  ba.TempRemainAmount
, AreaTempUsedAmount = ba.TempUsedAmount
, BranchName = branch.Name
, BranchAllocateAmount = bb.AllocateAmount
, BranchUsedAmount = bb.UsedAmount
, BranchRemainAmount = bb.RemainAmount
, BranchTempRemainAmount =  bb.TempRemainAmount
, BranchTempUsedAmount = bb.TempUsedAmount
, c.FromDate
, c.ToDate
, c.Number
FROM Budgets AS b
INNER JOIN Cycles AS c ON b.CycleId = c.Id
INNER JOIN BudgetZones AS bz ON bz.BudgetId = b.Id
INNER JOIN Zones AS z ON z.Id = bz.ZoneId
INNER JOIN Areas AS a ON bz.ZoneId =  a.ZoneId
INNER JOIN BudgetAreas AS ba ON ba.AreaId = a.Id AND ba.BudgetId = b.Id
INNER JOIN Branches AS branch ON branch.AreaId = ba.AreaId
INNER JOIN BudgetBranches AS bb ON bb.BranchId = branch.Id AND bb.BudgetId = b.Id
WHERE branch.IsActive = 1
AND InvestmentType = 3
AND CycleId = @CycleId
AND ISNULL(@ZoneId, z.Id) = z.Id
