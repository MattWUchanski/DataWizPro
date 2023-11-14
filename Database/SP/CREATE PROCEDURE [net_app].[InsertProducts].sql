SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [net_app].[InsertProducts]
    @ProductList net_app.ProductType READONLY
AS
BEGIN
    INSERT INTO net_app.Products (name, description, price, available)
    SELECT name, description, price, available
    FROM @ProductList;
END;

GO
