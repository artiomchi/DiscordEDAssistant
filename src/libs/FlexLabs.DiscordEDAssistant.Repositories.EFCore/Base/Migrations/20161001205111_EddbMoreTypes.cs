using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class EddbMoreTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Eddb_Systems",
                schema: "upload",
                table: "Eddb_Systems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Systems",
                schema: "eddb",
                table: "Systems");

            migrationBuilder.CreateTable(
                name: "Commodities_Categories",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stations_Types",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations_Types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Eddb_Commodities",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AveragePrice = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 255, nullable: true),
                    ID = table.Column<int>(nullable: false),
                    IsRare = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Commodities", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "Eddb_Stations",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Allegiance = table.Column<string>(maxLength: 512, nullable: true),
                    DistanceToStar = table.Column<int>(nullable: true),
                    Faction = table.Column<string>(maxLength: 512, nullable: true),
                    Government = table.Column<string>(maxLength: 512, nullable: true),
                    HasBlackmarket = table.Column<bool>(nullable: false),
                    HasCommodities = table.Column<bool>(nullable: false),
                    HasDocking = table.Column<bool>(nullable: false),
                    HasMarket = table.Column<bool>(nullable: false),
                    HasOutfitting = table.Column<bool>(nullable: false),
                    HasRearm = table.Column<bool>(nullable: false),
                    HasRefuel = table.Column<bool>(nullable: false),
                    HasRepair = table.Column<bool>(nullable: false),
                    HasShipyard = table.Column<bool>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                    IsPlanetary = table.Column<bool>(nullable: false),
                    MarketUpdatedAt = table.Column<DateTime>(nullable: true),
                    MaxLandingPadSize = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    OutfittingUpdatedAt = table.Column<DateTime>(nullable: true),
                    ShipyardUpdatedAt = table.Column<DateTime>(nullable: true),
                    State = table.Column<string>(maxLength: 512, nullable: true),
                    SystemID = table.Column<int>(nullable: false),
                    TypeID = table.Column<int>(nullable: true),
                    TypeName = table.Column<string>(maxLength: 512, nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Stations", x => x.PK);
                });

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
                    Ship = table.Column<string>(maxLength: 255, nullable: true),
                    StationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Stations_SellingShips", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "Commodities",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    AveragePrice = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    IsRare = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Commodities_Commodities_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "eddb",
                        principalTable: "Commodities_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Allegiance = table.Column<string>(maxLength: 255, nullable: true),
                    DistanceToStar = table.Column<int>(nullable: true),
                    Faction = table.Column<string>(maxLength: 255, nullable: true),
                    Government = table.Column<string>(maxLength: 255, nullable: true),
                    HasBlackmarket = table.Column<bool>(nullable: false),
                    HasCommodities = table.Column<bool>(nullable: false),
                    HasDocking = table.Column<bool>(nullable: false),
                    HasMarket = table.Column<bool>(nullable: false),
                    HasOutfitting = table.Column<bool>(nullable: false),
                    HasRearm = table.Column<bool>(nullable: false),
                    HasRefuel = table.Column<bool>(nullable: false),
                    HasRepair = table.Column<bool>(nullable: false),
                    HasShipyard = table.Column<bool>(nullable: false),
                    IsPlanetary = table.Column<bool>(nullable: false),
                    MarketUpdatedAt = table.Column<DateTime>(nullable: true),
                    MaxLandingPadSize = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    OutfittingUpdatedAt = table.Column<DateTime>(nullable: true),
                    ShipyardUpdatedAt = table.Column<DateTime>(nullable: true),
                    State = table.Column<string>(maxLength: 255, nullable: true),
                    SystemID = table.Column<int>(nullable: false),
                    TypeID = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stations_Stations_Types_TypeID",
                        column: x => x.TypeID,
                        principalSchema: "eddb",
                        principalTable: "Stations_Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stations_SellingModules",
                schema: "eddb",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleID = table.Column<int>(nullable: false),
                    StationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations_SellingModules", x => x.PK);
                    table.ForeignKey(
                        name: "FK_Stations_SellingModules_Stations_StationID",
                        column: x => x.StationID,
                        principalSchema: "eddb",
                        principalTable: "Stations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations_SellingShips",
                schema: "eddb",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ship = table.Column<string>(maxLength: 255, nullable: true),
                    StationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations_SellingShips", x => x.PK);
                    table.ForeignKey(
                        name: "FK_Stations_SellingShips_Stations_StationID",
                        column: x => x.StationID,
                        principalSchema: "eddb",
                        principalTable: "Stations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AlterColumn<byte>(
                name: "Rating",
                schema: "upload",
                table: "Eddb_Modules",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Rating",
                schema: "eddb",
                table: "Modules",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eddb_StarSystems",
                schema: "upload",
                table: "Eddb_Systems",
                column: "PK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarSystems",
                schema: "eddb",
                table: "Systems",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Commodities_CategoryID",
                schema: "eddb",
                table: "Commodities",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_TypeID",
                schema: "eddb",
                table: "Stations",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_SellingModules_StationID",
                schema: "eddb",
                table: "Stations_SellingModules",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_SellingShips_StationID",
                schema: "eddb",
                table: "Stations_SellingShips",
                column: "StationID");

            migrationBuilder.RenameTable(
                name: "Eddb_Systems",
                schema: "upload",
                newName: "Eddb_StarSystems");

            migrationBuilder.RenameTable(
                name: "Systems",
                schema: "eddb",
                newName: "StarSystems");

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
ALTER PROCEDURE [upload].[Eddb_Merge]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    EXEC [upload].[Eddb_Merge_Commodities]
    EXEC [upload].[Eddb_Merge_Modules]
    EXEC [upload].[Eddb_Merge_StarSystems]
    EXEC [upload].[Eddb_Merge_Stations]

END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [upload].[Eddb_Merge_Commodities]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[Commodities_Categories] AS [T]
	USING (
		SELECT DISTINCT [C].[CategoryID] AS [ID], [N].[CategoryName] AS [Name]
		FROM [upload].[Eddb_Commodities] AS [C]
		JOIN (
			SELECT [CategoryID], [CategoryName], ROW_NUMBER() OVER ( PARTITION BY [CategoryID] ORDER BY [PK] ) AS [Row]
			FROM [upload].[Eddb_Commodities] AS [C]
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

    MERGE [eddb].[Commodities] AS [T]
	USING [upload].[Eddb_Commodities] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name],
			[CategoryID] = [S].[CategoryID],
			[AveragePrice] = [S].[AveragePrice],
			[IsRare] = [S].[IsRare]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name], [CategoryID], [AveragePrice], [IsRare] )
		VALUES ( [ID], [Name], [CategoryID], [AveragePrice], [IsRare] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
");

            migrationBuilder.Sql(@"
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

            migrationBuilder.Sql(@"DROP PROCEDURE [upload].[Eddb_Merge_Systems]");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [upload].[Eddb_Merge_StarSystems]
	@FullSync AS BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[StarSystems] AS [T]
	USING [upload].[Eddb_StarSystems] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name],
			[X] = [S].[X],
			[Y] = [S].[Y],
			[Z] = [S].[Z],
			[IsPopulated] = [S].[IsPopulated]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name], [X], [Y], [Z], [IsPopulated] )
		VALUES ( [ID], [Name], [X], [Y], [Z], [IsPopulated] )
	WHEN NOT MATCHED BY SOURCE
		AND (@FullSync = 1 OR [T].[IsPopulated] = 1) THEN
		DELETE;

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Eddb_StarSystems",
                schema: "upload",
                table: "Eddb_StarSystems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarSystems",
                schema: "eddb",
                table: "StarSystems");

            migrationBuilder.DropTable(
                name: "Commodities",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Stations_SellingModules",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Stations_SellingShips",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Eddb_Commodities",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Eddb_Stations",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Eddb_Stations_SellingModules",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Eddb_Stations_SellingShips",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Commodities_Categories",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Stations",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Stations_Types",
                schema: "eddb");

            migrationBuilder.AlterColumn<char>(
                name: "Rating",
                schema: "upload",
                table: "Eddb_Modules",
                nullable: false);

            migrationBuilder.AlterColumn<char>(
                name: "Rating",
                schema: "eddb",
                table: "Modules",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eddb_Systems",
                schema: "upload",
                table: "Eddb_StarSystems",
                column: "PK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Systems",
                schema: "eddb",
                table: "StarSystems",
                column: "ID");

            migrationBuilder.RenameTable(
                name: "Eddb_StarSystems",
                schema: "upload",
                newName: "Eddb_Systems");

            migrationBuilder.RenameTable(
                name: "StarSystems",
                schema: "eddb",
                newName: "Systems");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Truncate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    TRUNCATE TABLE [upload].[Eddb_Modules]
    TRUNCATE TABLE [upload].[Eddb_Systems]

END
");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [upload].[Eddb_Merge]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    EXEC [upload].[Eddb_Merge_Modules]
    EXEC [upload].[Eddb_Merge_Systems]

END
");

            migrationBuilder.Sql(@"DROP PROCEDURE [upload].[Eddb_Merge_Commodities]");

            migrationBuilder.Sql(@"DROP PROCEDURE [upload].[Eddb_Merge_Stations]");

            migrationBuilder.Sql(@"DROP PROCEDURE [upload].[Eddb_Merge_StarSystems]");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [upload].[Eddb_Merge_Systems]
	@FullSync AS BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE [eddb].[Systems] AS [T]
	USING [upload].[Eddb_Systems] AS [S]
	ON [T].[ID] = [S].[ID]
	WHEN MATCHED THEN
		UPDATE SET
			[Name] = [S].[Name],
			[X] = [S].[X],
			[Y] = [S].[Y],
			[Z] = [S].[Z],
			[IsPopulated] = [S].[IsPopulated]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [Name], [X], [Y], [Z], [IsPopulated] )
		VALUES ( [ID], [Name], [X], [Y], [Z], [IsPopulated] )
	WHEN NOT MATCHED BY SOURCE
		AND (@FullSync = 1 OR [T].[IsPopulated] = 1) THEN
		DELETE;

END
");
        }
    }
}
