IF OBJECT_ID(N'RP_PivotPosm') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_PivotPosm
END

GO
CREATE PROC RP_PivotPosm
@UserId INT,
@CustomerId INT
AS
 --Request = 10,// Đề xuất
 --       ASMDeniedRequest = 20, // ASM từ chốt duyệt đề xuất
 --       AsmApprovedRequest = 30, // ASM duyệt đề xuất  
 --       RSMDeniedRequest = 40, // RSM từ chối duyệt đề xuất
 --       RsmApprovedRequest = 50, // RSM duyệt đề xuất
 --       TradeDeniedRequest = 60, // Trade từ chốt PYC
 --       TradeApprovedRequest = 70,// Trade xác nhận PYC hợp lệ
 --       DirectorDeniedRequest = 80, // Giám đốc từ chối duyệt
 --       DirectorApprovedRequest = 90, // Giám đọc duyệt PYC
 --       InvalidOrder = 100, //  Dơn hàng không hợp lệ
 --       ValidOrder = 150,
 --       ConfirmedProduce1 = 160, // Xác nhận sản xuất 1 (MKT)
 --       ConfirmedProduce2 = 170, // Xác nhận sản xuất 2 (SS)
 --       ConfirmedVendorProduce = 180, // Xác nhận cho NCC sản xuất
 --       Accepted = 190, // Nghiệm thu (SS)
 --       ConfirmAccept1 = 200, // Xác nhận nghiệm thu 1(ASM)
 --       ConfirmedAccept2 = 210 // Xác nhận nghiệm thu 2(Trade)

DECLARE @LowerDate DATETIME =  CAST((YEAR(GETDATE()) - 2) AS VARCHAR) + '-01-01'

SELECT 
    Code = i.Code,
    RegisterDate = i.CreationTime,
    PosmInvestmentItemId = iv.Id,
    PosmInvestmentId = i.Id,
    PosmInvestmentCode = i.Code,
    CustomerCode = c.Code,
    CustomerId = c.Id,
    ItemStatus = iv.Status,
    Status = i.Status,
	InvestmentStatus = CASE WHEN i.Status <= 100 THEN N'POSM Đề Xuất' ELSE N'POSM Đầu Tư' END,
    Email = c.Email,
    CustomerName = c.Name,
    MobilePhone = c.MobilePhone,
	[Year] = YEAR(i.RegisterDate),
    Address = c.Address,
    RegisterStaffName = s.Name,
    ZoneName = z.Name,
	AreaName = ar.Name,
    PosmItemId = pi.Id,
    PosmItemCode = pi.Code,
    PosmItemName = pi.Name,
    InvestmentAmount = IIF(iv.ActualTotalCost IS NOT NULL, iv.ActualTotalCost, iv.TotalCost),
    ActualTotalCost = iv.ActualTotalCost,
    TotalCost = iv.TotalCost,
    RemarkOfSales = iv.RemarkOfSales,
    RemarkOfCompany = iv.RemarkOfCompany,
	PosmClassName = pc.Name,
	PosmClassCode = pc.Code,
	PosmTypeName = IIF(pt.Id is null, 'N/A', pt.Name)
FROM PosmInvestments AS i
INNER JOIN PosmInvestmentItems AS iv ON i.Id = iv.PosmInvestmentId
INNER JOIN PosmItems AS pi ON iv.PosmItemId = pi.Id
INNER JOIN PosmClasses AS pc ON pi.PosmClassId = pc.Id
LEFT JOIN PosmTypes AS pt ON pi.PosmTypeId = pt.Id
INNER JOIN Customers AS c ON c.Id = i.CustomerId
INNER JOIN Zones AS z ON c.ZoneId = z.Id
INNER JOIN Areas AS ar ON c.AreaId = ar.Id
INNER JOIN Staffs AS s ON i.RegisterStaffId = s.Id
WHERE
i.CustomerId = @CustomerId
AND i.RegisterDate >= @LowerDate
AND i.Status NOT IN (20, 40, 60 , 80)

GO


IF OBJECT_ID(N'RP_PivotPosmHistory') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_PivotPosmHistory
END


GO
CREATE PROC RP_PivotPosmHistory
@UserId INT,
@CustomerId INT
AS
 --Request = 10,// Đề xuất
 --       ASMDeniedRequest = 20, // ASM từ chốt duyệt đề xuất
 --       AsmApprovedRequest = 30, // ASM duyệt đề xuất  
 --       RSMDeniedRequest = 40, // RSM từ chối duyệt đề xuất
 --       RsmApprovedRequest = 50, // RSM duyệt đề xuất
 --       TradeDeniedRequest = 60, // Trade từ chốt PYC
 --       TradeApprovedRequest = 70,// Trade xác nhận PYC hợp lệ
 --       DirectorDeniedRequest = 80, // Giám đốc từ chối duyệt
 --       DirectorApprovedRequest = 90, // Giám đọc duyệt PYC
 --       InvalidOrder = 100, //  Dơn hàng không hợp lệ
 --       ValidOrder = 150,
 --       ConfirmedProduce1 = 160, // Xác nhận sản xuất 1 (MKT)
 --       ConfirmedProduce2 = 170, // Xác nhận sản xuất 2 (SS)
 --       ConfirmedVendorProduce = 180, // Xác nhận cho NCC sản xuất
 --       Accepted = 190, // Nghiệm thu (SS)
 --       ConfirmAccept1 = 200, // Xác nhận nghiệm thu 1(ASM)
 --       ConfirmedAccept2 = 210 // Xác nhận nghiệm thu 2(Trade)

DECLARE @LowerDate DATETIME =  CAST((YEAR(GETDATE()) - 2) AS VARCHAR) + '-01-01'

SELECT 
    Code = i.Code,
    RegisterDate = i.CreationTime,
    PosmInvestmentItemId = iv.Id,
    PosmInvestmentId = i.Id,
    PosmInvestmentCode = i.Code,
    CustomerCode = c.Code,
    CustomerId = c.Id,
    ItemStatus = iv.Status,
    Status = i.Status,
	InvestmentStatus = CASE WHEN i.Status <= 100 THEN N'POSM Đề Xuất' ELSE N'POSM Đầu Tư' END,
    Email = c.Email,
    CustomerName = c.Name,
    MobilePhone = c.MobilePhone,
	[Year] = YEAR(i.RegisterDate),
    Address = c.Address,
    RegisterStaffName = s.Name,
    ZoneName = z.Name,
	AreaName = ar.Name,
    PosmItemId = pi.Id,
    PosmItemCode = pi.Code,
    PosmItemName = pi.Name,
    InvestmentAmount = IIF(iv.ActualTotalCost IS NOT NULL, iv.ActualTotalCost, iv.TotalCost),
    ActualTotalCost = iv.ActualTotalCost,
    TotalCost = iv.TotalCost,
    RemarkOfSales = iv.RemarkOfSales,
    RemarkOfCompany = iv.RemarkOfCompany,
	PosmClassName = pc.Name,
	PosmClassCode = pc.Code,
	PosmTypeName = IIF(pt.Id is null, 'N/A', pt.Name)
FROM PosmInvestments AS i
INNER JOIN PosmInvestmentItems AS iv ON i.Id = iv.PosmInvestmentId
INNER JOIN PosmItems AS pi ON iv.PosmItemId = pi.Id
INNER JOIN PosmClasses AS pc ON pi.PosmClassId = pc.Id
LEFT JOIN PosmTypes AS pt ON pi.PosmTypeId = pt.Id
INNER JOIN Customers AS c ON c.Id = i.CustomerId
INNER JOIN Zones AS z ON c.ZoneId = z.Id
INNER JOIN Areas AS ar ON c.AreaId = ar.Id
INNER JOIN Staffs AS s ON i.RegisterStaffId = s.Id
WHERE
i.CustomerId = @CustomerId
AND i.RegisterDate < @LowerDate
AND i.Status NOT IN (20, 40, 60 , 80)

GO

IF OBJECT_ID(N'RP_CustomerSales') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_CustomerSales
END

GO

CREATE PROC [dbo].RP_CustomerSales --RP_CustomerSales 0, 19561
@UserId int,
@CustomerId INT
AS

SELECT Year, Month = 'Tháng ' + IIF(LEN(Month) = 1, '0' + Month, Month), Amount 
FROM CustomerSales
WHERE CustomerId = @CustomerId
UNION 
SELECT Year, Month = 'Tháng ' + CAST(Month AS VARCHAR), Amount = 0
FROM 
	(
		SELECT DISTINCT YEAR 
		FROM CustomerSales
		WHERE CustomerId = @CustomerId
) AS y
CROSS JOIN (
	SELECT Month =  '01'
		UNION 
	SELECT Month =  '02'
		UNION 
	SELECT Month =  '03'
		UNION 
	SELECT Month =  '04'
		UNION 
	SELECT Month =  '05'
		UNION 
	SELECT Month =  '06'
		UNION 
	SELECT Month =  '07'
		UNION 
	SELECT Month =  '08'
		UNION 
	SELECT Month =  '09'
		UNION 
	SELECT Month =  '10'
		UNION 
	SELECT Month =  '11'
		UNION 
	SELECT Month =  '12'
) AS m
