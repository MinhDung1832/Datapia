USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_register]    Script Date: 15/09/2024 00:42:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_user_register]
    @user_code NVARCHAR(20)
  , @user_name NVARCHAR(150)
  , @password NVARCHAR(150)
  , @password_show NVARCHAR(150)
  , @email NVARCHAR(100)
AS
BEGIN
    INSERT INTO dbo.sys_user
    (
        user_code
      , user_name
      , user_password
      , password_show
      , email
      , status
      , create_by
      , create_date
    )
    VALUES
    (@user_code, @user_name, @password, @password_show, @email, 100, @user_code, GETDATE());

    INSERT INTO dbo.sys_user_role
    (
        user_code
      , role_code
      , role_name
      , status
      , create_by
      , create_date
    )
    VALUES
    (@user_code, 200, N'employee', 100, @user_code, GETDATE());
END;
GO
