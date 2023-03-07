CREATE TABLE [dbo].[Location](
	[ID]			INT				NOT NULL,
	[Name]			VARCHAR(20)		NOT NULL,
	[RackPosition]	VARCHAR(20)		NOT NULL,
	[Status]		BIT				NOT NULL,
	[CreateData]	DATETIME		NULL,
	[UpdateDate]	DATETIME		NULL,
	CONSTRAINT	[PK_Location] PRIMARY KEY CLUSTERED ([ID] ASC)
);


