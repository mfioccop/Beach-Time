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
