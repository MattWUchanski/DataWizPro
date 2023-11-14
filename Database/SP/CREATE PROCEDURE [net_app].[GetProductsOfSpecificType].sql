SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [net_app].[GetProductsOfSpecificType] 
    @name VARCHAR(100),
    @price DECIMAL(10, 2),
    @available BIT
AS
BEGIN
    SELECT * FROM net_app.Products 
    WHERE name = @name
    AND price = @price
    AND available = @available;
END;

GO
