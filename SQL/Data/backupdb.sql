DECLARE @dbName varchar(100),
		@fileName varchar(100),
		@path varchar(100),
		@dateTime varchar(8)

SET @dbName = 'ManagementContext'
SET @path = 'D:\BackupDb\'
SET @dateTime = CONVERT(VARCHAR(20),GETDATE(),112)
SET @fileName = @path + @dbName + '_' + @dateTime + '.bak'

BACKUP Database @dbName To Disk = @fileName