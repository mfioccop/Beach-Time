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

-- IUserStore procedures
IF OBJECT_ID('dbo.spUserCreate', 'P') IS NOT NULL
	DROP PROC dbo.spUserCreate
IF OBJECT_ID('dbo.spUserDelete', 'P') IS NOT NULL
	DROP PROC dbo.spUserDelete
IF OBJECT_ID('dbo.spUserFindAll', 'P') IS NOT NULL
	DROP PROC dbo.spUserFindAll
IF OBJECT_ID('dbo.spUserFindById', 'P') IS NOT NULL
	DROP PROC dbo.spUserFindById
IF OBJECT_ID('dbo.spUserFindByName', 'P') IS NOT NULL
	DROP PROC dbo.spUserFindByName
IF OBJECT_ID('dbo.spUserUpdate', 'P') IS NOT NULL
	DROP PROC dbo.spUserUpdate
GO
CREATE PROCEDURE [dbo].[spUserCreate]
(
	@userName VARCHAR(255),
	@firstName VARCHAR(255),
	@lastName VARCHAR(255),
	@email VARCHAR(255),
	@emailConfirmed BIT,
	@phoneNumber VARCHAR(16), --throws exception if phone number is truncated
	@phoneNumberConfirmed BIT,
	@accessFailedCount INT,
	@lockoutEndDateUtc DATETIME2,
	@lockoutEnabled BIT,
	@emailTwoFactorEnabled BIT,
	@googleAuthenticatorEnabled BIT,
	@googleAuthenticatorSecretKey VARCHAR(32),
	@passwordHash VARCHAR(149),
	@securityStamp UNIQUEIDENTIFIER
)
AS
BEGIN
	INSERT INTO Users (
		UserName,
		FirstName,
		LastName,
		Email,
		EmailConfirmed,
		PhoneNumber,
		PhoneNumberConfirmed,
		AccessFailedCount,
		LockoutEndDateUtc,
		LockoutEnabled,
		EmailTwoFactorEnabled,
		GoogleAuthenticatorEnabled,
		GoogleAuthenticatorSecretKey,
		PasswordHash,
		SecurityStamp
	) OUTPUT inserted.UserId VALUES (
		@userName,
		@firstName,
		@lastName,
		@email,
		@emailConfirmed,
		@phoneNumber,
		@phoneNumberConfirmed,
		@accessFailedCount,
		@lockoutEndDateUtc,
		@lockoutEnabled,
		@emailTwoFactorEnabled,
		@googleAuthenticatorEnabled,
		@googleAuthenticatorSecretKey,
		@passwordHash,
		@securityStamp
	)
END
GO
CREATE PROCEDURE [dbo].[spUserDelete]
(
	@userId INT
)
AS
BEGIN
	DELETE FROM Users
	WHERE UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spUserFindAll]
AS
BEGIN
	SELECT
		UserId,
		UserName,
		FirstName,
		LastName,
		Email,
		EmailConfirmed,
		PhoneNumber,
		PhoneNumberConfirmed,
		AccessFailedCount,
		LockoutEndDateUtc,
		LockoutEnabled,
		EmailTwoFactorEnabled,
		GoogleAuthenticatorEnabled,
		GoogleAuthenticatorSecretKey,
		PasswordHash,
		SecurityStamp
	FROM Users
END
GO
CREATE PROCEDURE [dbo].[spUserFindById]
(
	@userId INT
)
AS
BEGIN
	SELECT
		UserId,
		UserName,
		FirstName,
		LastName,
		Email,
		EmailConfirmed,
		PhoneNumber,
		PhoneNumberConfirmed,
		AccessFailedCount,
		LockoutEndDateUtc,
		LockoutEnabled,
		EmailTwoFactorEnabled,
		GoogleAuthenticatorEnabled,
		GoogleAuthenticatorSecretKey,
		PasswordHash,
		SecurityStamp
	FROM Users
	WHERE UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spUserFindByName]
(
	@userName VARCHAR(255)
)
AS
BEGIN
	SELECT
		UserId,
		UserName,
		FirstName,
		LastName,
		Email,
		EmailConfirmed,
		PhoneNumber,
		PhoneNumberConfirmed,
		AccessFailedCount,
		LockoutEndDateUtc,
		LockoutEnabled,
		EmailTwoFactorEnabled,
		GoogleAuthenticatorEnabled,
		GoogleAuthenticatorSecretKey,
		PasswordHash,
		SecurityStamp
	FROM Users
	WHERE UserName = @userName
END
GO
CREATE PROCEDURE [dbo].[spUserUpdate]
(
	@userId INT,
	@userName VARCHAR(255),
	@firstName VARCHAR(255),
	@lastName VARCHAR(255),
	@email VARCHAR(255),
	@emailConfirmed BIT,
	@phoneNumber VARCHAR(16), --throws exception if phone number is truncated on update
	@phoneNumberConfirmed BIT,
	@accessFailedCount INT,
	@lockoutEndDateUtc DATETIME2,
	@lockoutEnabled BIT,
	@emailTwoFactorEnabled BIT,
	@googleAuthenticatorEnabled BIT,
	@googleAuthenticatorSecretKey VARCHAR(32),
	@passwordHash VARCHAR(149),
	@securityStamp UNIQUEIDENTIFIER
)
AS
BEGIN
	UPDATE Users SET
		UserName = @userName,
		FirstName = @firstName,
		LastName = @lastName,
		Email = @email,
		EmailConfirmed = @emailConfirmed,
		PhoneNumber = @phoneNumber,
		PhoneNumberConfirmed = @phoneNumberConfirmed,
		AccessFailedCount = @accessFailedCount,
		LockoutEndDateUtc = @lockoutEndDateUtc,
		LockoutEnabled = @lockoutEnabled,
		EmailTwoFactorEnabled = @emailTwoFactorEnabled,
		GoogleAuthenticatorEnabled = @googleAuthenticatorEnabled,
		GoogleAuthenticatorSecretKey = @googleAuthenticatorSecretKey,
		PasswordHash = @passwordHash,
		SecurityStamp = @securityStamp
	WHERE UserId = @userId
END
GO
