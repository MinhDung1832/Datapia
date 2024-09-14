USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_update_info]    Script Date: 15/09/2024 00:42:55 ******/
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
