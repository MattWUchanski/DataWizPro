SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [net_app].[GetFilteredData] 
    @Param1 INT,
    @Param2 INT,
    @Param3 VARCHAR(50)
AS
BEGIN
    SELECT * FROM net_app.Users 
    WHERE id BETWEEN @Param1 AND @Param2 
    AND status = @Param3;
END;

GO
