CREATE PROCEDURE [upload].[Eddb_Truncate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    TRUNCATE TABLE [upload].[Eddb_Commodities]
    TRUNCATE TABLE [upload].[Eddb_Modules]
    TRUNCATE TABLE [upload].[Eddb_StarSystems]
    TRUNCATE TABLE [upload].[Eddb_Stations]
    TRUNCATE TABLE [upload].[Eddb_Stations_SellingModules]
    TRUNCATE TABLE [upload].[Eddb_Stations_SellingShips]

END
GO
