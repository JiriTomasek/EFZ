BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Role] 
                   WHERE [Name] = 'Admin')
                   
   BEGIN
       INSERT INTO [dbo].[Role] 
	   ([Name],[NormalizedName])
       VALUES ('Admin', 'ADMIN')
   END
END
BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Role] 
                   WHERE [Name] = 'Member')
                   
   BEGIN
       INSERT INTO [dbo].[Role] 
	   ([Name],[NormalizedName])
       VALUES ('Member', 'MEMBER')
   END
END

BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Role] 
                   WHERE [Name] = 'Guest')
                   
   BEGIN
       INSERT INTO [dbo].[Role] 
	   ([Name],[NormalizedName])
       VALUES ('Guest', 'GUEST')
   END
END


BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Company] 
                   WHERE [Id] = '43B0A232-FEAA-45FE-8C23-5EEA5D649782')
                   
   BEGIN
       INSERT INTO [dbo].[Company]  
	   ([Id],[Name], [IC],  [DIC])
       VALUES ('43B0A232-FEAA-45FE-8C23-5EEA5D649782', 'Company', 0, 0)
   END
END



BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Jobs] 
                   WHERE [Id] = 'D38FCDE8-9EED-43A8-BA7C-449A924799E5')
                   
   BEGIN
       INSERT INTO [dbo].[Jobs] 
	   ([Id],[JobName], [Length], [IsRun])
       VALUES ('D38FCDE8-9EED-43A8-BA7C-449A924799E5', 'XmlProcessing', '0:05', 0)
   END
END


BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Jobs] 
                   WHERE [Id] = '231CC6E8-542C-480D-93F4-831168F8869C')
                   
   BEGIN
       INSERT INTO [dbo].[Jobs] 
	   ([Id],[JobName], [Length], [IsRun])
       VALUES ('231CC6E8-542C-480D-93F4-831168F8869C', 'InvoiceCompletion', '0:10', 0)
   END
END

BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[Jobs] 
                   WHERE [Id] = 'FD6BFE1C-D763-43CB-8311-3833B4EBE031')
                   
   BEGIN
       INSERT INTO [dbo].[Jobs] 
	   ([Id],[JobName], [Length], [IsRun])
       VALUES ('FD6BFE1C-D763-43CB-8311-3833B4EBE031', 'AttachmentScanProcessing', '0:05', 0)
   END
END



BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[User] 
                   WHERE [UserName] = 'admin@admin.cz')
                   
   BEGIN
       INSERT INTO [dbo].[User]
           ([CustomerId]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[AuthenticationType]
           ,[IsAuthenticated]
           ,[Name])
     VALUES
           (NULL
           ,'admin@admin.cz'
           ,'ADMIN@ADMIN.CZ'
           ,'admin@admin.cz'
           ,0
           ,'AQAAAAEAACcQAAAAEGimZTJ8csCq9mWbqC1EhGIq0D+xmfFF0CWjtqg0JmSVwxa9JHLf+bGvn4nOuufu+w=='
           ,NULL
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,NULL
           ,0
           ,NULL)
   END
END

BEGIN
   IF NOT EXISTS (SELECT * FROM [dbo].[UserRole] 
                   WHERE [RoleId] = 1 AND [UserId]= 1)
                   
   BEGIN
       INSERT INTO [dbo].[UserRole]
           ([RoleId]
           ,[UserId])
     VALUES
           (1
           ,1)
   END
END
