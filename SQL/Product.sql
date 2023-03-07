CREATE TABLE [dbo].[Product](
	[ID]				INT				NOT NULL,
	[NumberID]			VARCHAR(50)		NOT NULL,
	[RFID]				VARCHAR(50)		NOT NULL,
	[Status]			VARCHAR(10)		NOT NULL,
	[LocationID]		INT				NOT NULL,
	[EngID]				INT				NOT NULL,
	[StockDate]			DATE			NOT NULL,
	[Life]				VARCHAR(10)		NOT NULL,
	[LastBorrowDate]	DATETIME		NULL,
	[LastReturnDate]	DATETIME		NULL,
	[UseLastDate]		DATETIME		NULL,	
	[Usable]			BIT				NOT NULL,
	[Overdue]			BIT				NOT NULL,
	[Spare1]			VARCHAR(100)	NULL,
	[Spare2]			VARCHAR(100)	NULL,
	[Spare3]			VARCHAR(100)	NULL,
	[Spare4]			VARCHAR(100)	NULL,
	[Spare5]			VARCHAR(100)	NULL,
	CONSTRAINT	[PK_Product] PRIMARY KEY CLUSTERED ([NumberID] ASC)
);

ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [FK_Product_Location] FOREIGN KEY ([LocationID])
      REFERENCES [dbo].[Location] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [FK_Product_Eng] FOREIGN KEY ([EngID])
      REFERENCES [dbo].[MeProduct] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;



ALTER TABLE [dbo].[Product] ADD [LastBorrowDate] DATETIME
ALTER TABLE [dbo].[Product] ADD [LastReturnDate] DATETIME

ALTER TABLE [dbo].[Product] DROP CONSTRAINT Id

ALTER TABLE [dbo].[Product] ADD CONSTRAINT PK_Product DEFAULT '' FOR Number


ALTER TABLE [Product] DROP COLUMN [Kind]