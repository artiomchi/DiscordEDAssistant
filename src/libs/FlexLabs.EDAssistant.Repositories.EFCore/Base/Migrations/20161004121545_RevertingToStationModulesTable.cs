using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlexLabs.EDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class RevertingToStationModulesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eddb_Stations_SellingModules",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleID = table.Column<int>(nullable: false),
                    StationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Stations_SellingModules", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "Eddb_Stations_SellingShips",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ship = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Stations_SellingShips", x => x.PK);
                });

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Truncate]
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
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Merge_Stations]
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
	USING [upload].[Eddb_Stations_SellingModules] AS [S]
	ON [T].[StationID] = [S].[StationID] AND [T].[ModuleID] = [S].[ModuleID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [StationID], [ModuleID] )
		VALUES ( [StationID], [ModuleID] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Stations_SellingShips] AS [T]
	USING [upload].[Eddb_Stations_SellingShips] AS [S]
	ON [T].[StationID] = [S].[StationID] AND [T].[Ship] = [S].[Ship]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [StationID], [Ship] )
		VALUES ( [StationID], [Ship] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [eddb].[Stations_SearchClosestByModule]
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
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eddb_Stations_SellingModules",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Eddb_Stations_SellingShips",
                schema: "upload");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Truncate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    TRUNCATE TABLE [upload].[Eddb_Commodities]
    TRUNCATE TABLE [upload].[Eddb_Modules]
    TRUNCATE TABLE [upload].[Eddb_StarSystems]
    TRUNCATE TABLE [upload].[Eddb_Stations]

END
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Merge_Stations]
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
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [eddb].[Stations_SearchClosestByModule]
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
		WHERE [SS].[IsPopulated] = 1
		AND (
			SELECT COUNT(0)
			FROM [eddb].[Stations_SellingModules] AS [SM]
			JOIN @ModuleIDs AS [MI] ON [SM].[ModuleID] = [MI].[ID]
			WHERE [SM].[StationID] = [S].[ID]) = (SELECT COUNT(0) FROM @ModuleIDs)
	) AS [X]
	ORDER BY [DistanceToSystem]

END
");
        }
    }
}
