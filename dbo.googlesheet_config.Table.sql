USE [PWBI_DEMO_shr]
GO
/****** Object:  Table [dbo].[googlesheet_config]    Script Date: 15/09/2024 00:42:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[googlesheet_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[spreadsheet_id] [nvarchar](250) NULL,
	[data_json] [nvarchar](max) NULL,
	[create_by] [nvarchar](50) NULL,
	[create_date] [datetime] NULL,
	[modify_by] [nvarchar](50) NULL,
	[modify_date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
