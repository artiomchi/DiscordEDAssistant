CREATE PROCEDURE [eddb].[Stations_SearchClosestByModule]
	@X AS REAL,
	@Y AS REAL,
	@Z AS REAL,
	@ModuleIDs AS TVP_INT READONLY,
	@Take AS INT = 25
AS
BEGIN

	SELECT TOP (@Take) [ID], [Name], [DistanceToStar], [MaxLandingPadSize], [IsPlanetary], [MarketUpdatedAt], [SystemID], [SystemName], [DistanceToSystem]
	FROM (
		SELECT [S].[ID], [S].[Name], [S].[DistanceToStar], [S].[MaxLandingPadSize], [S].[IsPlanetary], [S].[MarketUpdatedAt], [SS].[ID] AS [SystemID], [SS].[Name] AS [SystemName],
			SQRT(POWER([SS].[X] - @X, 2) + POWER([SS].[Y] - @Y, 2) + POWER([SS].[Z] - @Z, 2)) AS [DistanceToSystem]
		FROM [eddb].[Stations] AS [S]
		JOIN [eddb].[StarSystems] AS [SS] ON [S].[SystemID] = [SS].[ID]
		WHERE (
			SELECT COUNT(0)
			FROM [eddb].[Stations_SellingModules] AS [SM]
			JOIN @ModuleIDs AS [MI] ON [SM].[ModuleID] = [MI].[ID]
			WHERE [SM].[StationID] = [S].[ID]) = (SELECT COUNT(0) FROM @ModuleIDs)
	) AS [X]
	ORDER BY [DistanceToSystem]

END
GO
