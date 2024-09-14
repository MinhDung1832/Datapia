USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_config_get_list]    Script Date: 15/09/2024 00:42:55 ******/
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
