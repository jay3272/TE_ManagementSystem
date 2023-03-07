CREATE TABLE [dbo].[Department] (
	[ID]			INT				NOT NULL,
	[Name]			VARCHAR(20)		NOT NULL,
	[CreateData]	DATETIME		NULL,
	[UpdateDate]	DATETIME		NULL,
	CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([ID] ASC)
);