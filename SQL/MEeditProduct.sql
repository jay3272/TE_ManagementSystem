CREATE TABLE [dbo].[MeProduct](
	[ID]				INT				NOT NULL,
	[ProdName]			VARCHAR(100)	NOT NULL,
	[KindID]			INT				NOT NULL,
	[CustomerID]		INT				NOT NULL,
	[SupplierID]		INT				NOT NULL,
	[KpnID]				INT				NULL,
	[Quantity]			INT				NOT NULL,
	[ShiftTime]			INT				NOT NULL,
	[IsStock]			BIT				NOT NULL,
	[Pb]				BIT				NOT NULL,
	[Image]				VARCHAR(100)	NOT NULL,
	[ComList]			VARCHAR(100)	NOT NULL,
	[Spare1]			VARCHAR(100)	NULL,
	[Spare2]			VARCHAR(100)	NULL,
	[Spare3]			VARCHAR(100)	NULL,
	[Spare4]			VARCHAR(100)	NULL,
	[Spare5]			VARCHAR(100)	NULL,
	CONSTRAINT	[PK_MeProduct] PRIMARY KEY CLUSTERED ([ID] ASC)
);

ALTER TABLE [dbo].[MeProduct]
   ADD CONSTRAINT [FK_Product_Kind] FOREIGN KEY ([KindID])
      REFERENCES [dbo].[Kind] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[MeProduct]
   ADD CONSTRAINT [FK_Product_Customer] FOREIGN KEY ([CustomerID])
      REFERENCES [dbo].[Customer] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[MeProduct]
   ADD CONSTRAINT [FK_Product_Supplier] FOREIGN KEY ([SupplierID])
      REFERENCES [dbo].[Supplier] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

ALTER TABLE [dbo].[MeProduct]
   ADD CONSTRAINT [FK_Product_Kpn] FOREIGN KEY ([KpnID])
      REFERENCES [dbo].[KPN] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;


