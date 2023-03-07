CREATE TABLE [dbo].[Product](
	[ID]				INT				NOT NULL,
	[NumberID]			VARCHAR(50)		NOT NULL,
	[RFID]				VARCHAR(50)		NULL,
	[ProdName]			VARCHAR(100)	NOT NULL,
	[KindID]			INT				NOT NULL,
	[LocationID]		INT				NOT NULL,
	[SupplierID]		INT				NOT NULL,
	[Image]				VARCHAR(100)	NULL,
	[Pb]				BIT				NOT NULL,
	[Status]			VARCHAR(10)		NOT NULL,
	[StockDate]			DATE			NULL,
	[Life]				VARCHAR(10)		NULL,
	[Quantity]			INT				NOT NULL,
	[LastBorrowDate]	DATETIME		NULL,
	[LastReturnDate]	DATETIME		NULL,
	[UseLastDate]		DATETIME		NULL,	
	[Usable]			BIT				NOT NULL,
	[CreateData]		DATETIME		NULL,
	[UpdateDate]		DATETIME		NULL,
	CONSTRAINT	[PK_Product] PRIMARY KEY CLUSTERED ([ID] ASC)
);

ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [FK_Product_Kind] FOREIGN KEY ([KindID])
      REFERENCES [dbo].[Kind] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [FK_Product_Location] FOREIGN KEY ([LocationID])
      REFERENCES [dbo].[Location] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [FK_Product_Supplier] FOREIGN KEY ([SupplierID])
      REFERENCES [dbo].[Supplier] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;



ALTER TABLE [dbo].[Product] ADD [LastBorrowDate] DATETIME
ALTER TABLE [dbo].[Product] ADD [LastReturnDate] DATETIME

ALTER TABLE [dbo].[Product] DROP CONSTRAINT Id

ALTER TABLE [dbo].[Product] ADD CONSTRAINT PK_Product DEFAULT '' FOR Number


ALTER TABLE [Product] DROP COLUMN [Kind]