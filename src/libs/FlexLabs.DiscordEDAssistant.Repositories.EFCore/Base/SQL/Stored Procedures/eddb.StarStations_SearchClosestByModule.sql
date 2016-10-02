CREATE PROCEDURE [eddb].[Stations_SearchClosestByModule]
	@X AS MONEY,
	@Y AS MONEY,
	@Z AS MONEY,
	@ModuleIDs AS TVP_INT
AS
BEGIN

	SELECT [ID], [Name], [DistanceToStar], [SystemID], [SystemName], [DistanceToSystem]
	FROM (
		SELECT [S].[ID], [S].[Name], [S].[DistanceToStar], [SS].[ID] AS [SystemID], [SS].[Name] AS [SystemName],
			SQRT(POWER([SS].[X] - @X, 2) + POWER([SS].[Y] - @Y, 2) + POWER([SS].[Z] - @Z, 2)) AS [DistanceToSystem]
		FROM [eddb].[Stations] AS [S]
		JOIN [eddb].[StarSystems] AS [SS] ON [S].[SystemID] = [SS].[ID]
		WHERE EXISTS (
			SELECT 1
			FROM [eddb].[Stations_SellsModules] AS [SM]
			JOIN @ModuleIDs AS [MI] ON [SM].[ModuleID] = [MI].[ID]
			WHERE [SM].[StationID] = [S].[ID])
	) AS [X]
	ORDER BY [DistanceToSystem]

END
GO
