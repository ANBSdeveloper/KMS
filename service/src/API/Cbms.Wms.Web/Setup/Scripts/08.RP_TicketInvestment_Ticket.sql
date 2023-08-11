IF OBJECT_ID(N'RP_TicketInvestment_Ticket') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_Ticket
END

GO

CREATE PROC RP_TicketInvestment_Ticket
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
		, ConsumerName = ticket.ConsumerName
		, ConsumerPhone = ticket.ConsumerPhone
		, InvestmentCode = ticketInvest.Code
		, TicketCode = ticket.Code
		, PrintCount = ticket.PrintCount
		, LastPrintUser = [user].Name
		, IssueDate = ticket.IssueDate
		, OrderNumber = [order].OrderNumber
		, Api = [ticketDetail].Api
		, Status = CASE WHEN ticket.PrintDate IS NOT NULL THEN N'Đã In' ELSE N'Chưa In' END
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN Tickets AS ticket ON ticket.TicketInvestmentId = ticketInvest.Id
	INNER JOIN OrderTickets AS orderTicket ON orderTicket.TicketId = ticket.Id
	INNER JOIN Orders AS [order] ON  [order].Id = orderTicket.OrderId
	LEFT JOIN Users AS [user] ON [ticket].LastPrintUserId = [user].Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	OUTER APPLY (
		SELECT TOP 1 * FROM OrderDetails AS orderDetail WHERE orderDetail.OrderId = [order].Id AND orderDetail.QrCode = orderTicket.QrCode
	) AS [ticketDetail]
	WHERE  
	ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate                     
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
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
		, ConsumerName = ticket.ConsumerName
		, ConsumerPhone = ticket.ConsumerPhone
		, InvestmentCode = ticketInvest.Code
		, TicketCode = ticket.Code
		, PrintCount = ticket.PrintCount
		, LastPrintUser = [user].Name
		, IssueDate = ticket.IssueDate
		, OrderNumber = [order].OrderNumber
		, Api = [ticketDetail].Api
		, Status = CASE WHEN ticket.PrintDate IS NOT NULL THEN N'Đã In' ELSE N'Chưa In' END
	FROM TicketInvestments AS ticketInvest
	INNER JOIN Customers AS customer ON ticketInvest.CustomerId = customer.Id
	INNER JOIN Branches AS branch ON branch.Id = customer.BranchId
	INNER JOIN Tickets AS ticket ON ticket.TicketInvestmentId = ticketInvest.Id
	INNER JOIN OrderTickets AS orderTicket ON orderTicket.TicketId = ticket.Id
	INNER JOIN Orders AS [order] ON  [order].Id = orderTicket.OrderId
	LEFT JOIN Users AS [user] ON [ticket].LastPrintUserId = [user].Id
	LEFT JOIN Zones AS zone ON zone.Id = branch.ZoneId
	LEFT JOIN Areas AS area ON area.Id = branch.AreaId
	LEFT JOIN Provinces AS province ON province.Id = customer.ProvinceId
	CROSS APPLY fn_FindTeamLead(branch.Id) AS teamLead
	OUTER APPLY (
		SELECT TOP 1 * FROM OrderDetails AS orderDetail WHERE orderDetail.OrderId = [order].Id AND orderDetail.QrCode = orderTicket.QrCode
	) AS [ticketDetail]
	WHERE  
	ticketInvest.CreationTime >= @FromDate 
	AND ticketInvest.CreationTime <= @ToDate                     
	AND  EXISTS(
			SELECT TOP 1 *
			FROM CTE 
			WHERE CTE.TypeId = 1146 AND CTE.Id = branch.SalesOrgId)
	ORDER BY ticketInvest.CreationTime, customer.Code
END

