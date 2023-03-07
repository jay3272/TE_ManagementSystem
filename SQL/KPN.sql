CREATE TABLE [dbo].[KPN] (
	[ID]				INT				NOT NULL,
	[Name]				VARCHAR(100)	NOT NULL,
	[Spare1]			VARCHAR(100)	NULL,
	[Spare2]			VARCHAR(100)	NULL,
	[Spare3]			VARCHAR(100)	NULL,
	[Spare4]			VARCHAR(100)	NULL,
	[Spare5]			VARCHAR(100)	NULL,
	CONSTRAINT [PK_KPN] PRIMARY KEY CLUSTERED ([ID] ASC)
);

