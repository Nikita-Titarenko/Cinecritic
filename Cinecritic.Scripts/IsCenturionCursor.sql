DECLARE @CurrentUserId nvarchar(450);

DECLARE UserCursor CURSOR SCROLL FOR 
SELECT Id 
FROM ApplicationUsers
ORDER BY CreationDateTime ASC;

OPEN UserCursor;

FETCH ABSOLUTE 100 FROM UserCursor INTO @CurrentUserId;

WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE ApplicationUsers
    SET IsCenturion = 1
    WHERE Id = @CurrentUserId;

    FETCH RELATIVE 100 FROM UserCursor INTO @CurrentUserId;
END;

CLOSE UserCursor;
DEALLOCATE UserCursor;