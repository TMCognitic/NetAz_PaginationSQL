CREATE TABLE [dbo].[TestData]
(
	[Id] INT NOT NULL IDENTITY,
	[Value] NVARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME2(0) DEFAULT (SYSDATETIME()),
    CONSTRAINT [PK_TestData] PRIMARY KEY ([Id])
)
