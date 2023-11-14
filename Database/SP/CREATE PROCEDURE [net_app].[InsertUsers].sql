SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [net_app].[InsertUsers]
    @userTable [net_app].[UserType] READONLY
AS
BEGIN
    INSERT INTO [net_app].[Users] (username, email, created_at, status)
    SELECT username, email, created_at, status 
    FROM @userTable;
END;
GO
