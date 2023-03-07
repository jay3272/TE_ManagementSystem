CREATE TABLE [dbo].[Supplier] (
	[ID]				INT				NOT NULL,
	[Name]				VARCHAR(100)	NOT NULL,
	[Email]				VARCHAR(100)	NULL,
	[Phone]				VARCHAR(100)	NULL,
	[CreateData]		DATETIME		NULL,
	[UpdateDate]		DATETIME		NULL,
	CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([ID] ASC)
);