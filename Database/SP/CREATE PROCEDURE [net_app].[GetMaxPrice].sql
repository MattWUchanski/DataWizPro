SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [net_app].[GetMaxPrice] 
    @available BIT
AS
BEGIN
    SELECT MAX([price]) as MaksymalnaCena
    FROM [MyNewDatabase].[net_app].[Products]
    WHERE available = @available
END;

GO
