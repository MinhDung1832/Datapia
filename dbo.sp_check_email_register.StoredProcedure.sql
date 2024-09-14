USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_check_email_register]    Script Date: 15/09/2024 00:42:55 ******/
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
