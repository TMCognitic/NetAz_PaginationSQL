/*
Modèle de script de post-déploiement							
--------------------------------------------------------------------------------------
 Ce fichier contient des instructions SQL qui seront ajoutées au script de compilation.		
 Utilisez la syntaxe SQLCMD pour inclure un fichier dans le script de post-déploiement.			
 Exemple :      :r .\monfichier.sql								
 Utilisez la syntaxe SQLCMD pour référencer une variable dans le script de post-déploiement.		
 Exemple :      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


DECLARE @Number INT;

SET @Number = CONVERT(INT, RAND() * 1000);

WHILE (@Number < 100)
BEGIN
    SET @Number = CONVERT(INT, RAND() * 1000);
END

DECLARE @StartDate DATE = '1970/01/01';

DECLARE @Index INT = 0;

WHILE @Index < @Number
BEGIN
    INSERT INTO TestData ([Value], [CreatedDate]) VALUES (CONCAT(N'DATA ', @Index + 1), @StartDate);

    DECLARE @Part INT = CONVERT(INT, RAND() * 3);

    IF @Part = 0
        SET @StartDate = DATEADD(DAY, 1, @StartDate);
    ELSE IF @Part = 1
        SET @StartDate = DATEADD(Month, 1, @StartDate);
    ELSE
        SET @StartDate = DATEADD(Year, 1, @StartDate);

    SET @Index += 1;
END