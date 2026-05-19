USE CinecriticDb;
GO

DELETE FROM Reviews;
DELETE FROM WatchLists;
DELETE FROM MovieUsers;
DELETE FROM Movies;
DELETE FROM ApplicationUsers;
DELETE FROM MovieTypes;
GO

SET IDENTITY_INSERT MovieTypes ON;
INSERT INTO MovieTypes (Id, MovieTypeName) VALUES (1, 'Movie');
INSERT INTO MovieTypes (Id, MovieTypeName) VALUES (2, 'Series');
INSERT INTO MovieTypes (Id, MovieTypeName) VALUES (3, 'Cartoon');
SET IDENTITY_INSERT MovieTypes OFF;
GO

SET IDENTITY_INSERT Movies ON;
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (1, 'Interstellar', 'A team of explorers travel through a wormhole in space in an attempt to ensure humanity''s survival.', '2014-11-07', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (2, 'Whiplash', 'A promising young drummer enrolls at a music conservatory where an abusive instructor pushes him to the limit.', '2014-10-10', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (3, 'Oppenheimer', 'The story of J. Robert Oppenheimer and the development of the atomic bomb.', '2023-07-21', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (4, 'La La Land', 'A jazz musician and an aspiring actress fall in love while pursuing their dreams in Los Angeles.', '2016-12-09', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (5, 'Fight Club', 'An office worker and a soap maker form an underground fight club that evolves into something much more.', '1999-10-15', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (6, 'The Dark Knight', 'Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.', '2008-07-18', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (7, 'Spirited Away', 'During her family''s move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits.', '2001-07-20', 3, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (8, 'Dead Poets Society', 'English teacher John Keating inspires his students through his teaching of poetry.', '1989-06-02', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (9, 'Inside Out 2', 'Riley''s emotions return for a new adventure as she navigates adolescence.', '2025-06-14', 3, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (10, 'Shutter Island', 'A U.S. Marshal investigates the disappearance of a murderer who escaped from a hospital for the criminally insane.', '2010-02-19', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (11, 'Superman', 'Clark Kent, an alien orphan, grows up to become the superhero Superman.', '1978-12-15', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (12, 'Guardians of the Galaxy', 'A group of intergalactic criminals must pull together to stop a fanatical warrior.', '2014-08-01', 1, 0, 0, 0);
INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount) VALUES (13, 'Avengers', 'Earth''s mightiest heroes must come together to stop Loki and his alien army.', '2012-05-04', 1, 0, 0, 0);

DECLARE @MovieLoop INT = 0;
WHILE @MovieLoop < 100
BEGIN
    INSERT INTO Movies (Id, Title, Description, ReleaseDate, MovieTypeId, LikesCount, WatchListCount, WatchCount)
    VALUES (14 + @MovieLoop, 'TestMovie' + CAST(@MovieLoop AS VARCHAR(3)), NULL, '1951-03-13', 1, 0, 0, 0);
    SET @MovieLoop = @MovieLoop + 1;
END;
SET IDENTITY_INSERT Movies OFF;
GO

DECLARE @UserLoop INT = 0;
DECLARE @GeneratedUserId INT;
DECLARE @GeneratedMovieUserId INT;
DECLARE @MovieSelectionLoop INT;
DECLARE @TargetMovieId INT;
DECLARE @MaxMoviesToWatch INT;

WHILE @UserLoop < 50
BEGIN
    INSERT INTO ApplicationUsers (
        DisplayName, CreationDateTime, IsCenturion, UserName, NormalizedUserName, Email, NormalizedEmail, 
        EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
        PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, 
        LockoutEnabled, AccessFailedCount
    )
    VALUES (
        'test' + CAST(@UserLoop AS VARCHAR(2)), 
        SYSUTCDATETIME(),
        0,
        'test' + CAST(@UserLoop AS VARCHAR(2)) + '@gmail.com', 
        'TEST' + CAST(@UserLoop AS VARCHAR(2)) + '@GMAIL.COM', 
        'test' + CAST(@UserLoop AS VARCHAR(2)) + '@gmail.com', 
        'TEST' + CAST(@UserLoop AS VARCHAR(2)) + '@GMAIL.COM', 
        1, NULL, NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0
    );

    SET @GeneratedUserId = SCOPE_IDENTITY();

    SET @MaxMoviesToWatch = 1 + (@UserLoop % 5); 
    SET @MovieSelectionLoop = 0;

    WHILE @MovieSelectionLoop < @MaxMoviesToWatch
    BEGIN
        SET @TargetMovieId = 1 + ((@UserLoop + @MovieSelectionLoop * 3) % 13);

        IF NOT EXISTS (SELECT 1 FROM MovieUsers WHERE MovieId = @TargetMovieId AND ApplicationUserId = @GeneratedUserId)
        BEGIN
            INSERT INTO MovieUsers (MovieId, ApplicationUserId, WatchedDateTime, IsLiked, LikedDateTime, Rate)
            VALUES (@TargetMovieId, @GeneratedUserId, SYSUTCDATETIME(), (@UserLoop + @MovieSelectionLoop) % 2, CASE WHEN (@UserLoop + @MovieSelectionLoop) % 2 = 1 THEN SYSUTCDATETIME() ELSE NULL END, 5 + ((@UserLoop + @MovieSelectionLoop) % 6));

            SET @GeneratedMovieUserId = SCOPE_IDENTITY();

            IF (@UserLoop + @MovieSelectionLoop) % 3 = 0
            BEGIN
                INSERT INTO Reviews (MovieUserId, ReviewText, ReviewDateTime)
                VALUES (@GeneratedMovieUserId, 'TestReview_U' + CAST(@UserLoop AS VARCHAR(2)) + '_M' + CAST(@TargetMovieId AS VARCHAR(2)), SYSUTCDATETIME());
            END;
        END;

        SET @MovieSelectionLoop = @MovieSelectionLoop + 1;
    END;

    SET @UserLoop = @UserLoop + 1;
END;
GO

DECLARE @ExtraUserLoop INT = 0;
WHILE @ExtraUserLoop < 400
BEGIN
    INSERT INTO ApplicationUsers (
        DisplayName, CreationDateTime, IsCenturion, UserName, NormalizedUserName, Email, NormalizedEmail, 
        EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
        PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, 
        LockoutEnabled, AccessFailedCount
    )
    VALUES (
        'extra_test' + CAST(@ExtraUserLoop AS VARCHAR(3)), 
        SYSUTCDATETIME(),
        0,
        'extra_test' + CAST(@ExtraUserLoop AS VARCHAR(3)) + '@gmail.com', 
        'EXTRA_TEST' + CAST(@ExtraUserLoop AS VARCHAR(3)) + '@GMAIL.COM', 
        'extra_test' + CAST(@ExtraUserLoop AS VARCHAR(3)) + '@gmail.com', 
        'EXTRA_TEST' + CAST(@ExtraUserLoop AS VARCHAR(3)) + '@GMAIL.COM', 
        1, NULL, NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0
    );

    SET @ExtraUserLoop = @ExtraUserLoop + 1;
END;
GO

UPDATE m
SET m.WatchCount = ISNULL(src.TotalCount, 0),
    m.LikesCount = ISNULL(src.TotalLikes, 0)
FROM Movies m
LEFT JOIN (
    SELECT MovieId, 
           COUNT(*) AS TotalCount, 
           SUM(CASE WHEN IsLiked = 1 THEN 1 ELSE 0 END) AS TotalLikes
    FROM MovieUsers
    GROUP BY MovieId
) src ON m.Id = src.MovieId;
GO