CREATE TRIGGER TR_MovieUsers_UpdateLikesCount
ON MovieUsers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE m
    SET m.LikesCount = m.LikesCount + i.LikesIncrement,
        m.WatchCount = m.WatchCount + i.WatchIncrement
    FROM Movies m
    JOIN (
        SELECT MovieId, 
               SUM(LikesIncrement) AS LikesIncrement,
               SUM(WatchIncrement) AS WatchIncrement
        FROM (
            SELECT MovieId, 
                   CASE WHEN IsLiked = 1 THEN 1 ELSE 0 END AS LikesIncrement,
                   1 AS WatchIncrement
            FROM inserted
            UNION ALL
            SELECT MovieId, 
                   CASE WHEN IsLiked = 1 THEN -1 ELSE 0 END AS LikesIncrement,
                   -1 AS WatchIncrement
            FROM deleted
        ) sub
        GROUP BY MovieId
    ) i ON m.Id = i.MovieId;
END;
GO