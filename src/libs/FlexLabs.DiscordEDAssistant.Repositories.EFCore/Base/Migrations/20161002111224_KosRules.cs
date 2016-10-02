using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class KosRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KosSimpleRules",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorID = table.Column<long>(nullable: false),
                    Rule = table.Column<string>(nullable: false),
                    ServerID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KosSimpleRules", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "KosUserRules",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorID = table.Column<long>(nullable: false),
                    Rule = table.Column<string>(nullable: false),
                    ServerID = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KosUserRules", x => x.PK);
                });

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "upload",
                table: "Eddb_Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "eddb",
                table: "Modules",
                maxLength: 255,
                nullable: true);

            migrationBuilder.Sql(@"
CREATE TYPE [dbo].[TVP_INT] AS TABLE (
    [ID] INT NOT NULL);
");

            migrationBuilder.Sql(@"
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
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Merge_Modules]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[Modules_Categories] AS [T]
	USING (
		SELECT DISTINCT [C].[CategoryID] AS [ID], [N].[CategoryName] AS [Name]
		FROM [upload].[Eddb_Modules] AS [C]
		JOIN (
			SELECT [CategoryID], [CategoryName], ROW_NUMBER() OVER ( PARTITION BY [CategoryID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Modules] AS [C]
		) AS [N] ON [C].[CategoryID] = [N].[CategoryID] AND [N].[Row] = 1
		WHERE [C].[CategoryID] IS NOT NULL
	) AS [S] ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name] )
		VALUES ( [ID], [Name] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Modules_Groups] AS [T]
	USING (
		SELECT DISTINCT [C].[GroupID] AS [ID], [N].[GroupName] AS [Name]
		FROM [upload].[Eddb_Modules] AS [C]
		JOIN (
			SELECT [GroupID], [GroupName], ROW_NUMBER() OVER ( PARTITION BY [GroupID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Modules] AS [C]
		) AS [N] ON [C].[GroupID] = [N].[GroupID] AND [N].[Row] = 1
		WHERE [C].[GroupID] IS NOT NULL
	) AS [S] ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name] )
		VALUES ( [ID], [Name] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Modules] AS [T]
	USING [upload].[Eddb_Modules] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[CategoryID] = [S].[CategoryID],
			[Class] = [S].[Class],
			[GroupID] = [S].[GroupID],
			[Mass] = [S].[Mass],
			[MissileType] = [S].[MissileType],
			[Name] = [S].[Name],
			[Power] = [S].[Power],
			[Price] = [S].[Price],
			[Rating] = [S].[Rating],
			[Ship] = [S].[Ship],
			[WeaponMode] = [S].[WeaponMode],
			[FullName] = [S].[FullName]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode], [FullName] )
		VALUES ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode], [FullName] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KosSimpleRules");

            migrationBuilder.DropTable(
                name: "KosUserRules");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "upload",
                table: "Eddb_Modules");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "eddb",
                table: "Modules");

            migrationBuilder.Sql("DROP TYPE [dbo].[TVP_INT]");

            migrationBuilder.Sql("DROP PROCEDURE [eddb].[Stations_SearchClosestByModule]");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Merge_Modules]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[Modules_Categories] AS [T]
	USING (
		SELECT DISTINCT [C].[CategoryID] AS [ID], [N].[CategoryName] AS [Name]
		FROM [upload].[Eddb_Modules] AS [C]
		JOIN (
			SELECT [CategoryID], [CategoryName], ROW_NUMBER() OVER ( PARTITION BY [CategoryID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Modules] AS [C]
		) AS [N] ON [C].[CategoryID] = [N].[CategoryID] AND [N].[Row] = 1
		WHERE [C].[CategoryID] IS NOT NULL
	) AS [S] ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name] )
		VALUES ( [ID], [Name] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Modules_Groups] AS [T]
	USING (
		SELECT DISTINCT [C].[GroupID] AS [ID], [N].[GroupName] AS [Name]
		FROM [upload].[Eddb_Modules] AS [C]
		JOIN (
			SELECT [GroupID], [GroupName], ROW_NUMBER() OVER ( PARTITION BY [GroupID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Modules] AS [C]
		) AS [N] ON [C].[GroupID] = [N].[GroupID] AND [N].[Row] = 1
		WHERE [C].[GroupID] IS NOT NULL
	) AS [S] ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name] )
		VALUES ( [ID], [Name] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

    MERGE [eddb].[Modules] AS [T]
	USING [upload].[Eddb_Modules] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[CategoryID] = [S].[CategoryID],
			[Class] = [S].[Class],
			[GroupID] = [S].[GroupID],
			[Mass] = [S].[Mass],
			[MissileType] = [S].[MissileType],
			[Name] = [S].[Name],
			[Power] = [S].[Power],
			[Price] = [S].[Price],
			[Rating] = [S].[Rating],
			[Ship] = [S].[Ship],
			[WeaponMode] = [S].[WeaponMode]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode] )
		VALUES ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
");
        }
    }
}
