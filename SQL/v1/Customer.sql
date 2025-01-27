CREATE TABLE [dbo].[Customer] (
	[ID]			INT				NOT NULL,
	[Name]			VARCHAR(100)	NOT NULL,
	[CustCode]		VARCHAR(100)	NOT NULL,
	[CreateData]	DATETIME		NULL,
	[UpdateDate]	DATETIME		NULL,
	CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([ID] ASC)
);