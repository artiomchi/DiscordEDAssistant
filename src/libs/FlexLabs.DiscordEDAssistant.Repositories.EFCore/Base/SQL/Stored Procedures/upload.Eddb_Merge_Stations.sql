CREATE PROCEDURE [upload].[Eddb_Merge_Stations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[Stations_Types] AS [T]
	USING (
		SELECT DISTINCT [C].[TypeID] AS [ID], [N].[TypeName] AS [Name]
		FROM [upload].[Eddb_Stations] AS [C]
		JOIN (
			SELECT [TypeID], [TypeName], ROW_NUMBER() OVER ( PARTITION BY [TypeID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Stations] AS [C]
		) AS [N] ON [C].[TypeID] = [N].[TypeID] AND [N].[Row] = 1
		WHERE [C].[TypeID] IS NOT NULL
	) AS [S] ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name] )
		VALUES ( [ID], [Name] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Stations] AS [T]
	USING [upload].[Eddb_Stations] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
            [Name] = [S].[Name],
            [SystemID] = [S].[SystemID],
            [MaxLandingPadSize] = [S].[MaxLandingPadSize],
            [DistanceToStar] = [S].[DistanceToStar],
            [Faction] = [S].[Faction],
            [Government] = [S].[Government],
            [Allegiance] = [S].[Allegiance],
            [State] = [S].[State],
            [TypeID] = [S].[TypeID],
            [HasBlackmarket] = [S].[HasBlackmarket],
            [HasMarket] = [S].[HasMarket],
            [HasRefuel] = [S].[HasRefuel],
            [HasRepair] = [S].[HasRepair],
            [HasRearm] = [S].[HasRearm],
            [HasOutfitting] = [S].[HasOutfitting],
            [HasShipyard] = [S].[HasShipyard],
            [HasDocking] = [S].[HasDocking],
            [HasCommodities] = [S].[HasCommodities],
            [UpdatedAt] = [S].[UpdatedAt],
            [ShipyardUpdatedAt] = [S].[ShipyardUpdatedAt],
            [OutfittingUpdatedAt] = [S].[OutfittingUpdatedAt],
            [MarketUpdatedAt] = [S].[MarketUpdatedAt],
            [IsPlanetary] = [S].[IsPlanetary]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name], [SystemID], [MaxLandingPadSize], [DistanceToStar], [Faction], [Government], [Allegiance], [State], [TypeID], [HasBlackmarket], [HasMarket],
			[HasRefuel], [HasRepair], [HasRearm], [HasOutfitting], [HasShipyard], [HasDocking], [HasCommodities], [UpdatedAt], [ShipyardUpdatedAt], [OutfittingUpdatedAt],
			[MarketUpdatedAt], [IsPlanetary] )
		VALUES ( [ID], [Name], [SystemID], [MaxLandingPadSize], [DistanceToStar], [Faction], [Government], [Allegiance], [State], [TypeID], [HasBlackmarket], [HasMarket],
			[HasRefuel], [HasRepair], [HasRearm], [HasOutfitting], [HasShipyard], [HasDocking], [HasCommodities], [UpdatedAt], [ShipyardUpdatedAt], [OutfittingUpdatedAt],
			[MarketUpdatedAt], [IsPlanetary] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Stations_SellingModules] AS [T]
	USING (
		SELECT [S].[ID] AS [StationID], [X].[value] AS [ModuleID]
		FROM [upload].[Eddb_Stations] AS [S]
		CROSS APPLY (
			SELECT [value]
			FROM OPENJSON([S].[SellingModulesJson])
		) AS [X]
	) AS [S]
	ON [T].[StationID] = [S].[StationID] AND [T].[ModuleID] = [S].[ModuleID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [StationID], [ModuleID] )
		VALUES ( [StationID], [ModuleID] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Stations_SellingShips] AS [T]
	USING (
		SELECT [S].[ID] AS [StationID], [X].[value] AS [Ship]
		FROM [upload].[Eddb_Stations] AS [S]
		CROSS APPLY (
			SELECT [value]
			FROM OPENJSON([S].[SellingShipsJson])
		) AS [X]
	) AS [S]
	ON [T].[StationID] = [S].[StationID] AND [T].[Ship] = [S].[Ship]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [StationID], [Ship] )
		VALUES ( [StationID], [Ship] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
GO
