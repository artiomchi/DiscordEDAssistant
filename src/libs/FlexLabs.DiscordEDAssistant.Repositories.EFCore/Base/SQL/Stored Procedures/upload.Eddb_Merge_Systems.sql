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
GO
