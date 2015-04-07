-- Project procedures
IF OBJECT_ID('dbo.spProjectCreate', 'P') IS NOT NULL
	DROP PROC dbo.spProjectCreate
IF OBJECT_ID('dbo.spProjectDelete', 'P') IS NOT NULL
	DROP PROC dbo.spProjectDelete
IF OBJECT_ID('dbo.spProjectUpdate', 'P') IS NOT NULL
	DROP PROC dbo.spProjectUpdate
IF OBJECT_ID('dbo.spProjectFindAll', 'P') IS NOT NULL
	DROP PROC dbo.spProjectFindAll
IF OBJECT_ID('dbo.spProjectFindByProjectId', 'P') IS NOT NULL
	DROP PROC dbo.spProjectFindByProjectId
IF OBJECT_ID('dbo.spProjectFindByUserId', 'P') IS NOT NULL
	DROP PROC dbo.spProjectFindByUserId
IF OBJECT_ID('dbo.spProjectFindByName', 'P') IS NOT NULL
	DROP PROC dbo.spProjectFindByName
GO
CREATE PROCEDURE [dbo].[spProjectCreate]
(
	@userId INT,
	@name VARCHAR(255),
	@completed BIT
)
AS
BEGIN
	INSERT INTO dbo.Projects (UserId, Name, Completed) OUTPUT inserted.ProjectId VALUES (@userId, @name, @completed)
END
GO
CREATE PROCEDURE [dbo].[spProjectDelete]
(
	@projectId INT
)
AS
BEGIN
	DELETE FROM Projects WHERE ProjectId = @projectId
END
GO
CREATE PROCEDURE [dbo].[spProjectUpdate]
(
	@projectId INT,
	@userId INT,
	@name VARCHAR(255),
	@completed BIT
)
AS
BEGIN
	UPDATE Projects
	SET UserId = @userId, Name = @name, Completed = @completed
	WHERE ProjectId = @projectId
END
GO
CREATE PROCEDURE [dbo].[spProjectFindAll]
AS
BEGIN
	SELECT ProjectId, UserId, Name, Completed FROM Projects
END
GO
CREATE PROCEDURE [dbo].[spProjectFindByProjectId]
(
	@projectId INT
)
AS
BEGIN
	SELECT ProjectId, UserId, Name, Completed FROM Projects WHERE ProjectId = @projectId
END
GO
CREATE PROCEDURE [dbo].[spProjectFindByUserId]
(
	@userId INT
)
AS
BEGIN
	SELECT ProjectId, UserId, Name, Completed FROM Projects WHERE UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spProjectFindByName]
(
	@name INT
)
AS
BEGIN
	SELECT ProjectId, UserId, Name, Completed FROM Projects WHERE Name = @name
END
GO

-- FileInfo procedures
IF OBJECT_ID('dbo.spFileCreate', 'P') IS NOT NULL
	DROP PROC dbo.spFileCreate
IF OBJECT_ID('dbo.spFileDelete', 'P') IS NOT NULL
	DROP PROC dbo.spFileDelete
IF OBJECT_ID('dbo.spFileUpdate', 'P') IS NOT NULL
	DROP PROC dbo.spFileUpdate
IF OBJECT_ID('dbo.spFileFindByFileId', 'P') IS NOT NULL
	DROP PROC dbo.spFileFindByFileId
IF OBJECT_ID('dbo.spFileFindByUserId', 'P') IS NOT NULL
	DROP PROC dbo.spFileFindByUserId
GO
CREATE PROCEDURE [dbo].[spFileCreate]
(
	@userId INT,
	@path VARCHAR(900),
	@title VARCHAR(255),
	@description VARCHAR(8000)
)
AS
BEGIN
	INSERT INTO FileInfo (UserId, Path, Title, Description)
	OUTPUT inserted.FileId
	VALUES (@userId, @path, @title, @description)
END
GO
CREATE PROCEDURE [dbo].[spFileDelete]
(
	@fileId INT
)
AS
BEGIN
	DELETE FROM FileInfo WHERE FileId = @fileId
END
GO
CREATE PROCEDURE [dbo].[spFileUpdate]
(
	@fileId INT,
	@userId INT,
	@path VARCHAR(900),
	@title VARCHAR(255),
	@description VARCHAR(8000)
)
AS
BEGIN
	UPDATE FileInfo
	SET UserId = @userId, Path = @path, Title = @title, Description = @description
	WHERE FileId = @fileId
END
GO
CREATE PROCEDURE [dbo].[spFileFindByFileId]
(
	@fileId INT
)
AS
BEGIN
	SELECT FileId, UserId, Path, Title, Description
	FROM FileInfo
	WHERE FileId = @fileId
END
GO
CREATE PROCEDURE [dbo].[spFileFindByUserId]
(
	@userId INT
)
AS
BEGIN
	SELECT FileId, UserId, Path, Title, Description
	FROM FileInfo
	WHERE UserId = @userId
END
GO
