USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_config_delete]    Script Date: 15/09/2024 00:42:55 ******/
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
