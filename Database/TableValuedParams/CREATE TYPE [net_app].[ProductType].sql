CREATE TYPE [net_app].[ProductType] AS TABLE(
	[name] [varchar](100) NULL,
	[description] [varchar](255) NULL,
	[price] [decimal](10, 2) NULL,
	[available] [bit] NULL
)
GO
