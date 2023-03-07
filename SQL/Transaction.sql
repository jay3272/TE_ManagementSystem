CREATE TABLE [dbo].[ProductTransaction] (
	[ID]				INT				NOT NULL,
	[Opid]				VARCHAR(20)		NOT NULL,
	[ProductID]			VARCHAR(50)		NOT NULL,
	[BorrowReturn]		BIT				NOT NULL,
	[BorrowDay]			INT				NOT NULL,
	[RegisterDate]		DATETIME		NOT NULL,
	[Spare1]			VARCHAR(100)	NULL,
	[Spare2]			VARCHAR(100)	NULL,
	[Spare3]			VARCHAR(100)	NULL,
	[Spare4]			VARCHAR(100)	NULL,
	[Spare5]			VARCHAR(100)	NULL,
	CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED ([ID] ASC)
);

ALTER TABLE [dbo].[ProductTransaction]
   ADD CONSTRAINT [FK_ProductTransaction_Opid] FOREIGN KEY ([Opid])
      REFERENCES [dbo].[Employee] ([Opid])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[ProductTransaction]
   ADD CONSTRAINT [FK_ProductTransaction_Product] FOREIGN KEY ([ProductID])
      REFERENCES [dbo].[Product] ([NumberID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;
