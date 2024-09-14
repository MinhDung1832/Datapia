USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_info]    Script Date: 15/09/2024 00:42:55 ******/
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
		   password_show,
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
