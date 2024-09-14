USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_login]    Script Date: 15/09/2024 00:42:55 ******/
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
