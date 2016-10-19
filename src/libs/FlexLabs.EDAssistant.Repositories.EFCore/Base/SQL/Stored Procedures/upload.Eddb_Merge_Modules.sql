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
			[WeaponMode] = [S].[WeaponMode],
			[FullName] = [S].[FullName]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode], [FullName] )
		VALUES ( [ID], [CategoryID], [Class], [GroupID], [Mass], [MissileType], [Name], [Power], [Price], [Rating], [Ship], [WeaponMode], [FullName] )
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

END
GO
