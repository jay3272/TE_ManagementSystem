CREATE TABLE [dbo].[Kind](
	[ID]			INT				NOT NULL,
	[Name]			VARCHAR(100)	NOT NULL,
	[CreateData]	DATETIME		NULL,
	[UpdateDate]	DATETIME		NULL,
	CONSTRAINT	[PK_Kind] PRIMARY KEY CLUSTERED ([ID] ASC)
);
