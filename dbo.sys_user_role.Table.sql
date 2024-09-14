USE [PWBI_DEMO_shr]
GO
/****** Object:  Table [dbo].[sys_user_role]    Script Date: 15/09/2024 00:42:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_code] [nvarchar](20) NULL,
	[role_code] [int] NULL,
	[role_name] [nvarchar](20) NULL,
	[status] [int] NULL,
	[create_by] [nvarchar](20) NULL,
	[create_date] [datetime] NULL,
	[modify_by] [nvarchar](20) NULL,
	[modify_date] [datetime] NULL
) ON [PRIMARY]
GO
