CREATE TABLE [dbo].[ProductTransaction] (
	[ID]				INT				NOT NULL,
	[Opid]				VARCHAR(20)		NOT NULL,
	[ProductID]			VARCHAR(50)		NOT NULL,
	[CustomerID]		INT				NOT NULL,
	[Kpn]				VARCHAR(100)	NOT NULL,
	[BorrowReturn]		BIT				NOT NULL,
	[BorrowDay]			INT				NOT NULL,
	[RegisterDate]		DATETIME		NOT NULL,
	[Overdue]			BIT				NOT NULL,

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

ALTER TABLE [dbo].[ProductTransaction]
   ADD CONSTRAINT [FK_ProductTransaction_Customer] FOREIGN KEY ([CustomerID])
      REFERENCES [dbo].[Customer] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;
