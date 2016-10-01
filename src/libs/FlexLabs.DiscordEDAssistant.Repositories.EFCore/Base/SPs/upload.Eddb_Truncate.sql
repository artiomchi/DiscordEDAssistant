CREATE PROCEDURE [upload].[Eddb_Truncate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    TRUNCATE TABLE [upload].[Eddb_Modules]
    TRUNCATE TABLE [upload].[Eddb_Systems]

END
GO
