USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_config_update]    Script Date: 15/09/2024 00:42:55 ******/
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
