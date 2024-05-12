USE [Datapia]
GO
/****** Object:  Table [dbo].[sys_user]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_code] [nvarchar](20) NULL,
	[user_name] [nvarchar](150) NULL,
	[user_password] [nvarchar](150) NULL,
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
/****** Object:  StoredProcedure [dbo].[sp_check_email_register]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_check_email_register] @email NVARCHAR(100)
AS
BEGIN
    DECLARE @check INT =
            (
                SELECT COUNT(*)FROM dbo.sys_user WHERE UPPER(email) = UPPER(@email)
            );
    IF @check <= 0
        SELECT 'SUCCESS' msg;
    ELSE
        SELECT 'ERROR' msg;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_info]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
-- sp_user_info 'admin'
CREATE PROCEDURE [dbo].[sp_user_info] @user_code NVARCHAR(20)
AS
BEGIN
    SELECT id,
           user_code,
           user_name,
           user_password,
           email,
           phone_number phone_number,
           address address,
           nationality nationality,
           cccd cccd,
           issued_by issued_by,
           FORMAT(license_date,'dd/MM/yyyy') license_date,
           status,
           create_by,
           create_date,
           modify_by,
           modify_date
    FROM dbo.sys_user
    WHERE user_code = @user_code;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_login]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_user_login]
    @user_code NVARCHAR(20),
    @password NVARCHAR(250)
AS
BEGIN
    SELECT id,
           user_code,
           user_name,
           user_password,
           email,
           phone_number,
           address,
           nationality,
           cccd,
           issued_by,
           license_date,
           status,
           create_by,
           create_date,
           modify_by,
           modify_date
    FROM dbo.sys_user
    WHERE user_code = @user_code
          AND user_password = @password;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_register]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_user_register]
    @user_code NVARCHAR(20),
    @user_name NVARCHAR(150),
    @password NVARCHAR(150),
    @email NVARCHAR(100)
AS
BEGIN
    INSERT INTO dbo.sys_user
    (
        user_code,
        user_name,
        user_password,
        email,
        status,
        create_by,
        create_date
    )
    VALUES
    (@user_code, @user_name, @password, @email, 100, @user_code, GETDATE());
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_update_info]    Script Date: 5/12/2024 12:47:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_user_update_info]
    @user_code NVARCHAR(20),
    @user_name NVARCHAR(20),
    @user_password NVARCHAR(20),
    @email NVARCHAR(20),
    @phone_number NVARCHAR(20),
    @address NVARCHAR(20),
    @nationality NVARCHAR(20),
    @cccd NVARCHAR(20),
    @issued_by NVARCHAR(20),
    @license_date NVARCHAR(20)
AS
BEGIN
    UPDATE dbo.sys_user
    SET user_name = @user_name,
        --user_password = @user_password,
        --email = @email,
        phone_number = @phone_number,
        address = @address,
        nationality = @nationality,
        cccd = @cccd,
        issued_by = @issued_by,
        license_date = @license_date
    WHERE user_code = @user_code;
END;
GO
