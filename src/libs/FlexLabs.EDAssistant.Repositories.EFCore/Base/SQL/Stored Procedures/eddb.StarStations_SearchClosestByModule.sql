CREATE PROCEDURE [eddb].[Stations_SearchClosestByModule]
	@X AS REAL,
	@Y AS REAL,
	@Z AS REAL,
	@ModuleIDs AS TVP_INT READONLY,
	@Take AS INT = 25
AS
BEGIN

	DECLARE @ModuleCount AS INT = (SELECT COUNT(0) FROM @ModuleIDs)

	SELECT TOP (@Take)
		[S].[ID], [S].[Name], [S].[DistanceToStar], [S].[MaxLandingPadSize], [S].[IsPlanetary], [S].[MarketUpdatedAt],
		[SS].[ID] AS [SystemID], [SS].[Name] AS [SystemName], SQRT(POWER([SS].[X], 2) + POWER([SS].[Y], 2) + POWER([SS].[Z], 2)) AS [DistanceToSystem]
	FROM (
		SELECT [S].[ID]
		FROM [eddb].[Stations] AS [S]
		WHERE (
			SELECT COUNT(0)
			FROM [eddb].[Stations_SellingModules] AS [SM]
			JOIN @ModuleIDs AS [MI] ON [SM].[ModuleID] = [MI].[ID]
			WHERE [SM].[StationID] = [S].[ID]) = @ModuleCount
	) AS [X]
	JOIN [eddb].[Stations] AS [S] ON [X].[ID] = [S].[ID]
	JOIN [eddb].[StarSystems] AS [SS] ON [S].[SystemID] = [SS].[ID]
	WHERE [SS].[IsPopulated] = 1
	ORDER BY [DistanceToSystem]

END
GO
