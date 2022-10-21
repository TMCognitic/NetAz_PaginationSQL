CREATE PROCEDURE [dbo].[SP_GetHistory]
	@Page INT = 1,
	@LineCount INT = 25,
	@PageCount INT OUTPUT
AS
BEGIN
	Declare @Count int;

	--Compte le nombre d'enregistrement
	SELECT @Count = COUNT(*)
	FROM [dbo].[TestData];

	--Récupère le nombre de pages
	SET @PageCount = @Count / @LineCount;

	IF (@Count % @LineCount > 0)
		Set @PageCount += 1;

	--Récupère les données
	SELECT	[Id],
			[Value],
			[CreatedDate]
 	FROM [dbo].[TestData]
	ORDER BY [CreatedDate] DESC 
	OFFSET @LineCount * (@Page - 1) ROWS
	FETCH NEXT @LineCount ROWS ONLY;
END
