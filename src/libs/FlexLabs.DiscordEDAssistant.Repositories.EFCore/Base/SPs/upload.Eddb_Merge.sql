CREATE PROCEDURE [upload].[Eddb_Merge]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    EXEC [upload].[Eddb_Merge_Modules]
    EXEC [upload].[Eddb_Merge_Systems]

END
GO
