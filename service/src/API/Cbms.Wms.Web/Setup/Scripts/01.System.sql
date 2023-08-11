
IF OBJECT_ID(N'fn_FindTeamLead') IS NOT NULL
BEGIN
    DROP FUNCTION fn_FindTeamLead
END
GO
CREATE FUNCTION fn_FindTeamLead(@BranchId int)
RETURNS @User TABLE 
(
    UserId int PRIMARY KEY NOT NULL,
    UserName varchar(50),
    Name nvarchar(50) NULL
)
AS
BEGIN
	WITH CTE AS
	(
		SELECT SalesOrgs.*
		FROM   SalesOrgs
		WHERE Id = 730

		UNION ALL

		SELECT SalesOrgs.*
		FROM   SalesOrgs
		INNER JOIN CTE ON SalesOrgs.ID = CTE.ParentId
	)
	INSERT INTO @User(UserId, UserName, Name)
	SELECT TOP 1 UserId, UserName, [user].Name FROM CTE
	INNER JOIN UserAssignments AS assignment ON CTE.Id = assignment.SalesOrgId
	INNER JOIN Users AS [user] ON [user].Id = assignment.UserId
	WHERE [user].IsActive = 1 AND EXISTS(
		SELECT TOP 1 *
		FROM UserRoles AS userRole 
		INNER JOIN Roles AS role ON role.Id = userRole.RoleId
		WHERE userRole.UserId = [user].Id AND role.RoleName = 'TLCD'
	)ORDER BY TypeId DESC

	 RETURN;
END


GO

IF OBJECT_ID(N'dbo.[CustomerMap]') IS NOT NULL
BEGIN
    DROP TABLE dbo.[CustomerMap]
END

GO

CREATE TABLE [dbo].[CustomerMap](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [varchar](50) NOT NULL,
	[BranchCode] [varchar](50) NOT NULL,
	[SrStaffCode] [varchar](50) NOT NULL,
	[SsStaffCode] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CustomerMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
