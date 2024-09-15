USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_check_email_register]    Script Date: 15/09/2024 11:11:29 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_config_delete]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_config_delete]
    @userid NVARCHAR(50)
  , @id INT
AS
BEGIN
    UPDATE dbo.connection_config
    SET status = 300
      , modify_by = @userid
      , modify_date = GETDATE()
    WHERE id = @id;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_config_get_detail]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_config_get_detail]
    @userid NVARCHAR(20)
	,@id INT
AS
BEGIN
    SELECT id
         , data_from
         , acc_id_val
         , app_secret
         , access_token
         , FORMAT(get_from_date,'yyyy-MM-dd') get_from_date
         , data_to
         , acc_link
         , user_code
         , password
         , stop_using
         , acc_id
         , acc_name
         , camp_id
         , camp_name
         , ad_group_id
         , ad_group_name
         , currency_unit
         , start_date
         , end_date
         , year_old
         , nation
         , region_id
         , region_name
         , gender
         , performance
         , status
         , create_by
         , create_date
         , modify_by
         , modify_date FROM dbo.connection_config
	WHERE id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_config_get_list]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_config_get_list]
    @userid NVARCHAR(20)
  , @data_from NVARCHAR(50)
  , @acc_id_val NVARCHAR(50)
  , @status NVARCHAR(20)
AS
BEGIN
    SELECT id
         , data_from
         , acc_id_val
         , app_secret
         , access_token
         , FORMAT(get_from_date, 'dd/MM/yyyy') get_from_date
         , data_to
         , acc_link
         , user_code
         , password
         , stop_using
         , acc_id
         , acc_name
         , camp_id
         , camp_name
         , ad_group_id
         , ad_group_name
         , currency_unit
         , start_date
         , end_date
         , year_old
         , nation
         , region_id
         , region_name
         , gender
         , performance
         , status
         , create_by
         , FORMAT(create_date, 'dd/MM/yyyy') create_date
         , modify_by
         , modify_date
    FROM dbo.connection_config
    WHERE create_by = @userid
          AND status <> 300
          AND
          (   @data_from = ''
              OR data_from = @data_from)
          AND
          (   @acc_id_val = ''
              OR acc_id_val = acc_id_val)
          AND
          (   @status = ''
              OR status = @status);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_config_insert]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_config_insert]
    @userid NVARCHAR(20)
  , @id INT
  , @data_from NVARCHAR(250)
  , @acc_id_val NVARCHAR(250)
  , @app_secret NVARCHAR(250)
  , @access_token NVARCHAR(500)
  , @app_id NVARCHAR(250)
  , @get_from_date DATE
--, @data_to NVARCHAR(50)
--, @acc_link NVARCHAR(150)
--, @user_code NVARCHAR(50)
--, @password NVARCHAR(150)
--, @stop_using BIT
--, @acc_id BIT
--, @acc_name BIT
--, @camp_id BIT
--, @camp_name BIT
--, @ad_group_id BIT
--, @ad_group_name BIT
--, @currency_unit BIT
--, @start_date BIT
--, @end_date BIT
--, @year_old BIT
--, @nation BIT
--, @region_id BIT
--, @region_name BIT
--, @gender BIT
--, @performance BIT
AS
BEGIN
    INSERT INTO dbo.connection_config
    (
        data_from
      , acc_id_val
      , app_secret
      , access_token
      , app_id
      , get_from_date
      --, data_to
      --, acc_link
      --, user_code
      --, password
      --, stop_using
      --, acc_id
      --, acc_name
      --, camp_id
      --, camp_name
      --, ad_group_id
      --, ad_group_name
      --, currency_unit
      --, start_date
      --, end_date
      --, year_old
      --, nation
      --, region_id
      --, region_name
      --, gender
      --, performance
      , status
      , create_by
      , create_date
    )
    VALUES
    (   @data_from, @acc_id_val, @app_secret, @access_token, @app_id
      , @get_from_date
      --, @data_to, @acc_link, @user_code
      --  , @password, @stop_using, @acc_id, @acc_name, @camp_id, @camp_name, @ad_group_id, @ad_group_name, @currency_unit
      --  , @start_date, @end_date, @year_old, @nation, @region_id, @region_name, @gender, @performance
      , 100, @userid, GETDATE());
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_config_update]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_config_update]
    @userid NVARCHAR(20)
  , @id INT
  , @data_from NVARCHAR(50)
  , @acc_id_val NVARCHAR(50)
  , @app_secret NVARCHAR(50)
  , @access_token NVARCHAR(500)
  , @get_from_date DATE
  , @data_to NVARCHAR(50)
  , @acc_link NVARCHAR(150)
  , @user_code NVARCHAR(50)
  , @password NVARCHAR(150)
  , @stop_using BIT
  , @acc_id BIT
  , @acc_name BIT
  , @camp_id BIT
  , @camp_name BIT
  , @ad_group_id BIT
  , @ad_group_name BIT
  , @currency_unit BIT
  , @start_date BIT
  , @end_date BIT
  , @year_old BIT
  , @nation BIT
  , @region_id BIT
  , @region_name BIT
  , @gender BIT
  , @performance BIT
AS
BEGIN
    UPDATE dbo.connection_config
    SET data_from = @data_from
      , acc_id_val = @acc_id_val
      , app_secret = @app_secret
      , access_token = @access_token
      , get_from_date = @get_from_date
      , data_to = @data_to
      , acc_link = @acc_link
      , user_code = @user_code
      , password = @password
      , stop_using = @stop_using
      , acc_id = @acc_id
      , acc_name = @acc_name
      , camp_id = @camp_id
      , camp_name = @camp_name
      , ad_group_id = @ad_group_id
      , ad_group_name = @ad_group_name
      , currency_unit = @currency_unit
      , start_date = @start_date
      , end_date = @end_date
      , year_old = @year_old
      , nation = @nation
      , region_id = @region_id
      , region_name = @region_name
      , gender = @gender
      , performance = @performance
      , modify_by = @userid
      , modify_date = GETDATE()
    WHERE id = @id;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_googlesheet_get]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_googlesheet_get] @userid NVARCHAR(50)
AS
BEGIN
    SELECT id
         , spreadsheet_id
         , REVERSE(SUBSTRING(REVERSE(data_json), 1, CHARINDEX('\', REVERSE(data_json)) - 1)) data_json
         , create_by
         , FORMAT(create_date, 'dd/MM/yyyy') create_date
         , modify_by
         , modify_date
    FROM dbo.googlesheet_config
    WHERE create_by = @userid;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_googlesheet_insert]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_googlesheet_insert]
    @userid NVARCHAR(50)
  , @spreadsheet_id NVARCHAR(250)
  , @data_json NVARCHAR(250)
AS
BEGIN
    INSERT INTO dbo.googlesheet_config
    (
        spreadsheet_id
      , data_json
      , create_by
      , create_date
    )
    VALUES
    (@spreadsheet_id, @data_json, @userid, GETDATE());
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_change_status]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_user_change_status]
    @userid NVARCHAR(20)
  , @role_code NVARCHAR(20)
  , @id INT
  , @status INT
AS
BEGIN
    IF @role_code <> '200'
    BEGIN
        UPDATE dbo.sys_user
        SET status = @status
          , modify_by = @userid
          , modify_date = GETDATE()
        WHERE id = @id;
		SELECT 'SUCCESS' msg;
    END
	ELSE
    BEGIN
        SELECT 'ERROR' msg;
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_get_list]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_user_get_list]
    @userid NVARCHAR(20)
	,@role_code NVARCHAR(20)
AS
BEGIN
IF	@role_code <> '200'
BEGIN
    SELECT A.* FROM dbo.sys_user A LEFT JOIN dbo.sys_user_role B ON A.user_code = B.user_code
	WHERE B.role_code = 200
END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_user_info]    Script Date: 15/09/2024 11:11:29 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_user_login]    Script Date: 15/09/2024 11:11:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
CREATE PROCEDURE [dbo].[sp_user_login]
    @user_code NVARCHAR(20)
  , @password NVARCHAR(250)
AS
BEGIN
    SELECT A.id
         , A.user_code
         , A.user_name
         , A.user_password
         , A.email
         , A.phone_number
         , A.address
         , A.nationality
         , A.cccd
         , A.issued_by
         , A.license_date
         , A.status
         , A.create_by
         , A.create_date
         , A.modify_by
         , A.modify_date
         , B.role_code
         , B.role_name
    FROM dbo.sys_user A
        LEFT JOIN dbo.sys_user_role B
            ON A.user_code = B.user_code
    WHERE A.user_code = @user_code
          AND user_password = @password
          AND A.status = 100;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_user_register]    Script Date: 15/09/2024 11:11:29 ******/
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
    (@user_code, @user_name, @password, @password_show, @email, 200, @user_code, GETDATE());

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
/****** Object:  StoredProcedure [dbo].[sp_user_update_info]    Script Date: 15/09/2024 11:11:29 ******/
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
