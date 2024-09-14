USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_config_get_detail]    Script Date: 15/09/2024 00:42:55 ******/
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
