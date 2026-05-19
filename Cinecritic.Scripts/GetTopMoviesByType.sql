CREATE OR ALTER FUNCTION dbo.GetTopMoviesByType(
    @MovieTypeId INT, 
    @MinRating DECIMAL(4,2),
    @PageNumber INT,
    @PageSize INT,
    @ApplicationUserId NVARCHAR(450) = NULL
)
RETURNS @ResultTable TABLE (
    MovieId INT,
    Title NVARCHAR(200),
    ReleaseDate DATE,
    AverageRating DECIMAL(4,2),
    TotalWatches INT,
    IsLikedByCurrentUser BIT
)
AS
BEGIN
    DECLARE @RowsToSkip INT = (@PageNumber - 1) * @PageSize;

    INSERT INTO @ResultTable
    SELECT 
        m.Id AS MovieId,
        m.Title,
        m.ReleaseDate,
        dbo.GetMovieAverageRating(m.Id) AS AverageRating,
        m.WatchCount,
        CASE 
            WHEN @ApplicationUserId IS NOT NULL AND EXISTS (
                SELECT 1 FROM MovieUsers 
                WHERE MovieId = m.Id AND ApplicationUserId = @ApplicationUserId AND IsLiked = 1
            ) THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END AS IsLikedByCurrentUser
    FROM Movies m
    LEFT JOIN MovieUsers mu ON m.Id = mu.MovieId AND mu.Rate IS NOT NULL
    WHERE m.MovieTypeId = @MovieTypeId
    GROUP BY m.Id, m.Title, m.ReleaseDate, m.WatchCount
    HAVING dbo.GetMovieAverageRating(m.Id) >= @MinRating
    ORDER BY AverageRating DESC, m.Title ASC
    OFFSET @RowsToSkip ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    RETURN;
END;
GO