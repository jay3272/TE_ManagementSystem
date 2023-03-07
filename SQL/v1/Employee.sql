CREATE TABLE [dbo].[Employee] (
	[Opid]				VARCHAR(20)		NOT NULL,
	[Name]				VARCHAR(4)		NOT NULL,
	[Email]				VARCHAR(50)		NOT NULL,
	[RankID]			TINYINT			NULL,
	[DepartmentID]		INT				NOT NULL,
	[IsActive]			VARCHAR(2)		NOT NULL,	
	[CreateData]		DATETIME		NULL,
	[UpdateDate]		DATETIME		NULL,
	CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Opid] ASC)
);

ALTER TABLE [dbo].[Employee]
   ADD CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentID])
      REFERENCES [dbo].[Department] ([ID])
      ON DELETE CASCADE
      ON UPDATE CASCADE
;