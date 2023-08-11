IF EXISTS (SELECT TOP 1 1 FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[DBO].[FR_SPLITSTRINGMAX]') AND XTYPE IN (N'FN', N'IF', N'TF'))
DROP FUNCTION [DBO].[FR_SPLITSTRINGMAX]
GO

CREATE FUNCTION [dbo].[fr_SplitStringMAX] 
(
    -- Add the parameters for the function here
    @myString varchar(MAX),
    @deliminator varchar(10)
)
RETURNS 
@ReturnTable TABLE 
(
    -- Add the column definitions for the TABLE variable here
    [id] [int] IDENTITY(1,1) NOT NULL,
    [part] [varchar](100) NULL
)
AS
BEGIN
        Declare @iSpaces int
        Declare @part varchar(100)

        --initialize spaces
        Select @iSpaces = charindex(@deliminator,@myString,0)
        While @iSpaces > 0

        Begin
            Select @part = substring(@myString,0,charindex(@deliminator,@myString,0))

            Insert Into @ReturnTable(part)
            Select @part

    Select @myString = substring(@mystring,charindex(@deliminator,@myString,0)+ len(@deliminator),len(@myString))


            Select @iSpaces = charindex(@deliminator,@myString,0)
        end

        If len(@myString) > 0
            Insert Into @ReturnTable
            Select @myString

    RETURN 
END



GO



IF OBJECT_ID(N'RP_TicketInvestment_PrintTicket') IS NOT NULL
BEGIN
    DROP PROCEDURE RP_TicketInvestment_PrintTicket
END

GO

CREATE PROC [dbo].[RP_TicketInvestment_PrintTicket] -- [RP_Ticket_Print_Result] '76,77'    
@UserId INT, -- bua a noi la luôn có userid của ngươi dfung hien tai :)
@TicketId varchar(max)  
AS  
BEGIN
	SELECT tisket.Code, tisket.ConsumerName, tisket.ConsumerPhone 
	FROM Tickets AS tisket 
	INNER JOIN dbo.fr_SplitStringMAX(@TicketId,',') AS b ON tisket.Id = CAST(b.part AS INT)
END