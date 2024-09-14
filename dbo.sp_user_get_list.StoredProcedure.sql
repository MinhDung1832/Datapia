USE [PWBI_DEMO_shr]
GO
/****** Object:  StoredProcedure [dbo].[sp_user_get_list]    Script Date: 15/09/2024 00:42:55 ******/
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
