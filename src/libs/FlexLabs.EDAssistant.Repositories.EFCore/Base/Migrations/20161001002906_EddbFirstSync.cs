using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlexLabs.EDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class EddbFirstSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "eddb");

            migrationBuilder.EnsureSchema(
                name: "upload");

            migrationBuilder.CreateTable(
                name: "Modules_Categories",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Modules_Groups",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules_Groups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Systems",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    IsPopulated = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Eddb_Modules",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 255, nullable: true),
                    Class = table.Column<byte>(nullable: false),
                    GroupID = table.Column<int>(nullable: false),
                    GroupName = table.Column<string>(maxLength: 255, nullable: true),
                    ID = table.Column<int>(nullable: false),
                    Mass = table.Column<float>(nullable: false),
                    MissileType = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Power = table.Column<float>(nullable: false),
                    Price = table.Column<int>(nullable: true),
                    Rating = table.Column<char>(nullable: false),
                    Ship = table.Column<string>(nullable: true),
                    WeaponMode = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Modules", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "Eddb_Systems",
                schema: "upload",
                columns: table => new
                {
                    PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ID = table.Column<int>(nullable: false),
                    IsPopulated = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eddb_Systems", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "eddb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    Class = table.Column<byte>(nullable: false),
                    GroupID = table.Column<int>(nullable: false),
                    Mass = table.Column<float>(nullable: false),
                    MissileType = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Power = table.Column<float>(nullable: false),
                    Price = table.Column<int>(nullable: true),
                    Rating = table.Column<char>(nullable: false),
                    Ship = table.Column<string>(maxLength: 255, nullable: true),
                    WeaponMode = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Modules_Modules_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "eddb",
                        principalTable: "Modules_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modules_Modules_Groups_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "eddb",
                        principalTable: "Modules_Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CategoryID",
                schema: "eddb",
                table: "Modules",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_GroupID",
                schema: "eddb",
                table: "Modules",
                column: "GroupID");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [upload].[Eddb_Truncate]
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
CREATE PROCEDURE [upload].[Eddb_Merge_Modules]
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

            migrationBuilder.Sql(@"
CREATE PROCEDURE [upload].[Eddb_Merge]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    EXEC [upload].[Eddb_Merge_Modules]
    EXEC [upload].[Eddb_Merge_Systems]

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Systems",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Eddb_Modules",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Eddb_Systems",
                schema: "upload");

            migrationBuilder.DropTable(
                name: "Modules_Categories",
                schema: "eddb");

            migrationBuilder.DropTable(
                name: "Modules_Groups",
                schema: "eddb");

            migrationBuilder.Sql("DROP PROCEDURE [upload].[Eddb_Merge]");
            migrationBuilder.Sql("DROP PROCEDURE [upload].[Eddb_Merge_Modules]");
            migrationBuilder.Sql("DROP PROCEDURE [upload].[Eddb_Merge_Systems]");
            migrationBuilder.Sql("DROP PROCEDURE [upload].[Eddb_Merge_Truncate]");
        }
    }
}
