USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_config_insert]    Script Date: 15/09/2024 00:42:55 ******/
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
