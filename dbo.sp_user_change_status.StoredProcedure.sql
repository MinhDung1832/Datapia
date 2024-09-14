USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_change_status]    Script Date: 15/09/2024 00:42:55 ******/
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
