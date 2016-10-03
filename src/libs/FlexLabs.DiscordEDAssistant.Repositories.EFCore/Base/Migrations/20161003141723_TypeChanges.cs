using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class TypeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SellingShipsJson",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SellingModulesJson",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Government",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Allegiance",
                schema: "upload",
                table: "Eddb_Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_StarSystems",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                schema: "upload",
                table: "Eddb_Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "upload",
                table: "Eddb_Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                schema: "upload",
                table: "Eddb_Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Commodities",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                schema: "upload",
                table: "Eddb_Commodities",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Stations_Types",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Ship",
                schema: "eddb",
                table: "Stations_SellingShips",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "eddb",
                table: "Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Government",
                schema: "eddb",
                table: "Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "eddb",
                table: "Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Allegiance",
                schema: "eddb",
                table: "Stations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "StarSystems",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules_Groups",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules_Categories",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Ship",
                schema: "eddb",
                table: "Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "eddb",
                table: "Modules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Commodities",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Commodities_Categories",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.Sql(@"
CREATE NONCLUSTERED INDEX [IX_StarSystems_Name]
	ON [eddb].[StarSystems] ( [Name] )
");

            migrationBuilder.Sql(@"
CREATE NONCLUSTERED INDEX [IX_StarSystems_Name__OnlyPopulated]
	ON [eddb].[StarSystems] ( [Name] )
	WHERE [IsPopulated] = 1
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SellingShipsJson",
                schema: "upload",
                table: "Eddb_Stations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SellingModulesJson",
                schema: "upload",
                table: "Eddb_Stations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Government",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Allegiance",
                schema: "upload",
                table: "Eddb_Stations",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_StarSystems",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                schema: "upload",
                table: "Eddb_Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "upload",
                table: "Eddb_Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                schema: "upload",
                table: "Eddb_Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "upload",
                table: "Eddb_Commodities",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                schema: "upload",
                table: "Eddb_Commodities",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Stations_Types",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ship",
                schema: "eddb",
                table: "Stations_SellingShips",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "eddb",
                table: "Stations",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Stations",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Government",
                schema: "eddb",
                table: "Stations",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "eddb",
                table: "Stations",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Allegiance",
                schema: "eddb",
                table: "Stations",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "StarSystems",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules_Groups",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules_Categories",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ship",
                schema: "eddb",
                table: "Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "eddb",
                table: "Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Commodities",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "eddb",
                table: "Commodities_Categories",
                maxLength: 255,
                nullable: true);

            migrationBuilder.Sql("DROP INDEX [IX_StarSystems_Name] ON [eddb].[StarSystems]");

            migrationBuilder.Sql("DROP INDEX [IX_StarSystems_Name__OnlyPopulated] ON [eddb].[StarSystems]");

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
		WHERE (
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
