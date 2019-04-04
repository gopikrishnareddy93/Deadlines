/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


insert into Deadlines
(Name,DueDate,Description)
values
('test1', '05/25/2019', 'test description');


CREATE USER [deadlinesAppUser] FOR LOGIN [deadlinesAppUser]
GO

EXEC sp_addrolemember N'db_datareader', N'deadlinesAppUser'  
GO  

EXEC sp_addrolemember N'db_datawriter', N'deadlinesAppUser'  
GO  
