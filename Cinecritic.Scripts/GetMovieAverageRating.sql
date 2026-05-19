CREATE OR ALTER FUNCTION dbo.GetMovieAverageRating (@MovieId INT)
RETURNS DECIMAL(4, 2)
AS
BEGIN
    DECLARE @AvgRate DECIMAL(4, 2);

    SELECT @AvgRate = AVG(CAST(Rate AS DECIMAL(4, 2)))
    FROM MovieUsers
    WHERE MovieId = @MovieId;

    RETURN ISNULL(@AvgRate, 0.00);
END;