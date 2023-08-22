USE [KMS]
GO

ALTER TABLE PosmInvestments
ADD OperationPhoto1 nvarchar(200) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationPhoto2 nvarchar(200) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationPhoto3 nvarchar(200) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationPhoto4 nvarchar(200) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationDate datetime2(0) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationLink nvarchar(200) NULL
GO

ALTER TABLE PosmInvestments
ADD OperationNote nvarchar(1000) NULL
GO