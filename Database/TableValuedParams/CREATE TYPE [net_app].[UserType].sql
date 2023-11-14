CREATE TYPE [net_app].[UserType] AS TABLE(
	[username] [varchar](50) NULL,
	[email] [varchar](100) NULL,
	[created_at] [datetime] NULL,
	[status] [varchar](20) NULL
)
GO
