USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'CinecriticDb')
BEGIN
    ALTER DATABASE CinecriticDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CinecriticDb;
END;
GO

CREATE DATABASE CinecriticDb;
GO

USE CinecriticDb;
GO

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(256) NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (Id)
);
GO

CREATE TABLE ApplicationUsers (
    Id INT IDENTITY(1,1) NOT NULL,
    DisplayName NVARCHAR(30) NOT NULL,
    CreationDateTime DATETIME2 NOT NULL DEFAULT (SYSUTCDATETIME()),
    Email NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(200) NULL,
    CONSTRAINT PK_ApplicationUsers PRIMARY KEY (Id)
);
GO

CREATE TABLE MovieTypes (
    Id INT IDENTITY(1,1) NOT NULL,
    MovieTypeName NVARCHAR(200) NOT NULL,
    CONSTRAINT PK_MovieTypes PRIMARY KEY (Id)
);
GO

CREATE TABLE ApplicationUserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT PK_ApplicationUserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_ApplicationUserRoles_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ApplicationUserRoles_ApplicationUsers_UserId FOREIGN KEY (UserId) REFERENCES ApplicationUsers (Id) ON DELETE CASCADE
);
GO

CREATE TABLE Movies (
    Id INT IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NULL,
    ReleaseDate DATE NULL,
    MovieTypeId INT NOT NULL,
    CONSTRAINT PK_Movies PRIMARY KEY (Id),
    CONSTRAINT FK_Movies_MovieTypes_MovieTypeId FOREIGN KEY (MovieTypeId) REFERENCES MovieTypes (Id) ON DELETE CASCADE
);
GO

CREATE TABLE MovieUsers (
    MovieId INT NOT NULL,
    UserId INT NOT NULL,
    IsLiked BIT NOT NULL,
    Rate INT NULL,
    CONSTRAINT PK_MovieUsers PRIMARY KEY (MovieId, UserId),
    CONSTRAINT FK_MovieUsers_ApplicationUsers_UserId FOREIGN KEY (UserId) REFERENCES ApplicationUsers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MovieUsers_Movies_MovieId FOREIGN KEY (MovieId) REFERENCES Movies (Id) ON DELETE CASCADE
);
GO

CREATE TABLE WatchLists (
    MovieId INT NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT PK_WatchLists PRIMARY KEY (MovieId, UserId),
    CONSTRAINT FK_WatchLists_ApplicationUsers_UserId FOREIGN KEY (UserId) REFERENCES ApplicationUsers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_WatchLists_Movies_MovieId FOREIGN KEY (MovieId) REFERENCES Movies (Id) ON DELETE CASCADE
);
GO

CREATE TABLE Reviews (
    MovieId INT NOT NULL,
    UserId INT NOT NULL,
    ReviewText NVARCHAR(500) NOT NULL,
    ReviewDateTime DATETIME2 NOT NULL,
    CONSTRAINT PK_Reviews PRIMARY KEY (MovieId, UserId),
    CONSTRAINT FK_Reviews_MovieUsers_MovieId_UserId FOREIGN KEY (MovieId, UserId) REFERENCES MovieUsers (MovieId, UserId) ON DELETE CASCADE
);
GO