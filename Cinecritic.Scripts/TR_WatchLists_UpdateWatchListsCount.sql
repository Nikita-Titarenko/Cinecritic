CREATE TRIGGER TR_WatchLists_UpdateWatchListsCount
ON WatchLists
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE m
    SET m.WatchListCount = m.WatchListCount + i.Increment
    FROM Movies m
    JOIN (
        SELECT MovieId, SUM(Increment) AS Increment
        FROM (
            SELECT MovieId, 1 AS Increment FROM inserted
            UNION ALL
            SELECT MovieId, -1 AS Increment FROM deleted
        ) sub
        GROUP BY MovieId
    ) i ON m.Id = i.MovieId;
END;
GO