CREATE TABLE [dbo].[LabelRule](
	[ID]			INT				NOT NULL,
	[ProcessKindID]	INT				NOT NULL,
	[KindID]		INT				NOT NULL,
	[LabelRule]		VARCHAR(100)	NOT NULL,
	CONSTRAINT	[PK_LabelRule] PRIMARY KEY CLUSTERED ([ID] ASC)
);

ALTER TABLE [dbo].[LabelRule]
   ADD CONSTRAINT [FK_LabelRule_Kind] FOREIGN KEY ([KindID])
      REFERENCES [dbo].[Kind] ([ID])
;

ALTER TABLE [dbo].[LabelRule]
   ADD CONSTRAINT [FK_LabelRule_ProcessKind] FOREIGN KEY ([ProcessKindID])
      REFERENCES [dbo].[KindProcess] ([ID])
;
