USE [PWBI_DEMO_shr]
GO
/****** Object:  Table [dbo].[sys_user]    Script Date: 15/09/2024 00:42:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_code] [nvarchar](20) NULL,
	[user_name] [nvarchar](150) NULL,
	[user_password] [nvarchar](150) NULL,
	[password_show] [nvarchar](150) NULL,
	[email] [nvarchar](50) NULL,
	[phone_number] [nvarchar](20) NULL,
	[address] [nvarchar](250) NULL,
	[nationality] [nvarchar](50) NULL,
	[cccd] [nvarchar](50) NULL,
	[issued_by] [nvarchar](150) NULL,
	[license_date] [date] NULL,
	[status] [int] NULL,
	[create_by] [nvarchar](20) NULL,
	[create_date] [datetime] NULL,
	[modify_by] [nvarchar](20) NULL,
	[modify_date] [datetime] NULL
) ON [PRIMARY]
GO
