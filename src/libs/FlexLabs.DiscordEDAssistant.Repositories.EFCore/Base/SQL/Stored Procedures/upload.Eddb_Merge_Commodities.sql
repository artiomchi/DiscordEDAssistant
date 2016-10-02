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
GO
