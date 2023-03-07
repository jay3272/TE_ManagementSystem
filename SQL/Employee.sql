CREATE TABLE [dbo].[Employee] (
	[Opid]				VARCHAR(20)		NOT NULL,
	[Name]				VARCHAR(50)		NOT NULL,
	[Email]				VARCHAR(50)		NOT NULL,
	[RankID]			TINYINT			NOT NULL,
	[DepartmentID]		INT				NOT NULL,
	[IsActive]			VARCHAR(2)		NOT NULL,
	[Spare1]			VARCHAR(100)	NULL,
	[Spare2]			VARCHAR(100)	NULL,
	[Spare3]			VARCHAR(100)	NULL,
	[Spare4]			VARCHAR(100)	NULL,
	[Spare5]			VARCHAR(100)	NULL,
	CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Opid] ASC)
);

ALTER TABLE [dbo].[Employee]
   ADD CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentID])
      REFERENCES [dbo].[Department] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;