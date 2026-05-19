CREATE OR ALTER PROCEDURE dbo.UpsertMovieUserLikeAndRating
    @MovieId INT,
    @ApplicationUserId NVARCHAR(450),
    @IsLiked BIT,
    @Rate INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Rate IS NOT NULL AND (@Rate < 1 OR @Rate > 10)
        BEGIN
            RAISERROR('Rating must be between 1 and 10.', 16, 1);
        END

        IF EXISTS (SELECT 1 FROM MovieUsers WHERE MovieId = @MovieId AND ApplicationUserId = @ApplicationUserId)
        BEGIN
            UPDATE MovieUsers
            SET IsLiked = @IsLiked,
                Rate = @Rate
            WHERE MovieId = @MovieId AND ApplicationUserId = @ApplicationUserId;
        END
        ELSE
        BEGIN
            INSERT INTO MovieUsers (MovieId, ApplicationUserId, IsLiked, Rate, WatchedDateTime, LikedDateTime)
            VALUES (@MovieId, @ApplicationUserId, @IsLiked, @Rate, SYSDATETIME(), SYSDATETIME());
        END

        IF @IsLiked = 1 OR @Rate IS NOT NULL
        BEGIN
            DELETE FROM WatchLists
            WHERE MovieId = @MovieId AND ApplicationUserId = @ApplicationUserId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO