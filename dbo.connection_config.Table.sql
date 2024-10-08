USE [PWBI_DEMO_shr]
GO
/****** Object:  Table [dbo].[connection_config]    Script Date: 15/09/2024 00:42:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[connection_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[data_from] [nvarchar](50) NULL,
	[acc_id_val] [nvarchar](50) NULL,
	[app_secret] [nvarchar](50) NULL,
	[access_token] [nvarchar](500) NULL,
	[get_from_date] [date] NULL,
	[data_to] [nvarchar](50) NULL,
	[acc_link] [nvarchar](250) NULL,
	[user_code] [nvarchar](50) NULL,
	[password] [nvarchar](150) NULL,
	[stop_using] [bit] NULL,
	[acc_id] [bit] NULL,
	[acc_name] [bit] NULL,
	[camp_id] [bit] NULL,
	[camp_name] [bit] NULL,
	[ad_group_id] [bit] NULL,
	[ad_group_name] [bit] NULL,
	[currency_unit] [bit] NULL,
	[start_date] [bit] NULL,
	[end_date] [bit] NULL,
	[year_old] [bit] NULL,
	[nation] [bit] NULL,
	[region_id] [bit] NULL,
	[region_name] [bit] NULL,
	[gender] [bit] NULL,
	[performance] [bit] NULL,
	[status] [int] NULL,
	[create_by] [nvarchar](20) NULL,
	[create_date] [datetime] NULL,
	[modify_by] [nvarchar](20) NULL,
	[modify_date] [datetime] NULL,
	[app_id] [nvarchar](250) NULL
) ON [PRIMARY]
GO
