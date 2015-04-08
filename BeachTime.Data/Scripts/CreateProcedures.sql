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

-- IUserLoginStore procedures
IF OBJECT_ID('dbo.spUserLoginAdd', 'P') IS NOT NULL
	DROP PROC dbo.spUserLoginAdd
IF OBJECT_ID('dbo.spUserLoginFind', 'P') IS NOT NULL
	DROP PROC dbo.spUserLoginFind
IF OBJECT_ID('dbo.spUserLoginGet', 'P') IS NOT NULL
	DROP PROC dbo.spUserLoginGet
IF OBJECT_ID('dbo.spUserLoginRemove', 'P') IS NOT NULL
	DROP PROC dbo.spUserLoginRemove
GO
CREATE PROCEDURE [dbo].[spUserLoginAdd]
(
	@userId INT,
	@loginProvider VARCHAR(255),
	@providerKey VARCHAR(255)
)
AS
BEGIN
	INSERT INTO ExternalLogins (
		UserId,
		LoginProvider,
		ProviderKey
	) VALUES (
		@userId,
		@loginProvider,
		@providerKey
	)
END
GO
CREATE PROCEDURE [dbo].[spUserLoginFind]
(
	@loginProvider VARCHAR(255),
	@providerKey VARCHAR(255)
)
AS
BEGIN
	SELECT
		u.UserId,
		u.UserName,
		u.FirstName,
		u.LastName,
		u.Email,
		u.EmailConfirmed,
		u.PhoneNumber,
		u.PhoneNumberConfirmed,
		u.AccessFailedCount,
		u.LockoutEndDateUtc,
		u.LockoutEnabled,
		u.EmailTwoFactorEnabled,
		u.GoogleAuthenticatorEnabled,
		u.GoogleAuthenticatorSecretKey,
		u.PasswordHash,
		u.SecurityStamp
	FROM Users u
	INNER JOIN ExternalLogins l
		ON l.UserId = u.UserId
	WHERE l.LoginProvider = @loginProvider
		AND l.ProviderKey = @providerKey
END
GO
CREATE PROCEDURE [dbo].[spUserLoginGet]
(
	@userId INT
)
AS
BEGIN
	SELECT
		LoginProvider,
		ProviderKey
	FROM ExternalLogins
	WHERE UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spUserLoginRemove]
(
	@userId INT,
	@loginProvider VARCHAR(255),
	@providerKey VARCHAR(255)
)
AS
BEGIN
	DELETE FROM ExternalLogins
	WHERE UserId = @userId
		AND LoginProvider = @loginProvider
		AND ProviderKey = @providerKey
END
GO

-- IUserEmailStore procedures
IF OBJECT_ID('dbo.spUserEmailFind', 'P') IS NOT NULL
	DROP PROC dbo.spUserEmailFind
GO
CREATE PROCEDURE [dbo].[spUserEmailFind]
(
	@email VARCHAR(255)
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
	WHERE Email = @email
END
GO

-- IUserRoleStore procedures
IF OBJECT_ID('dbo.spUserRoleAdd', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleAdd
IF OBJECT_ID('dbo.spUserRoleGet', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleGet
IF OBJECT_ID('dbo.spUserRoleRemove', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRemove
IF OBJECT_ID('dbo.spUserRoleRequestChange', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRequestChange
IF OBJECT_ID('dbo.spUserRoleRequestGet', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRequestGet
IF OBJECT_ID('dbo.spUserRoleRequestGetAll', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRequestGetAll
IF OBJECT_ID('dbo.spUserRoleRequestGetById', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRequestGetById
IF OBJECT_ID('dbo.spUserRoleGetAll', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleGetAll
IF OBJECT_ID('dbo.spUserRoleRequestRemove', 'P') IS NOT NULL
	DROP PROC dbo.spUserRoleRequestRemove
GO
CREATE PROCEDURE [dbo].[spUserRoleAdd]
(
	@userId INT,
	@roleName VARCHAR(255)
)
AS
BEGIN
	INSERT INTO UserRoles (UserId, RoleId)
		SELECT DISTINCT @userId AS UserId, RoleId
		FROM Users, Roles
		WHERE Roles.Name = @roleName
END
GO
CREATE PROCEDURE [dbo].[spUserRoleGet]
(
	@userId INT
)
AS
BEGIN
	SELECT Roles.Name
	FROM Roles
	JOIN UserRoles ON UserRoles.RoleId = Roles.RoleId
	WHERE UserRoles.UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRemove]
(
	@userId INT,
	@roleName VARCHAR(255)
)
AS
BEGIN
	DELETE ur
	FROM UserRoles ur
	JOIN Roles r
		ON r.RoleId = ur.RoleId
		AND ur.UserId = @userId
		AND r.Name = @roleName
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRequestChange]
(
	@userId INT,
	@roleName VARCHAR(255)
)
AS
BEGIN
	INSERT INTO RoleChangeRequests (UserId, RoleId, RequestDate)
	OUTPUT Inserted.RequestId, Inserted.RequestDate
	SELECT @userId, Roles.RoleId, SYSDATETIME()
	FROM Roles
	WHERE Roles.Name = @roleName
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRequestGet]
(
	@userId INT
)
AS
BEGIN
	SELECT
		rcr.RequestId,
		rcr.UserId,
		r.Name
	AS
		RoleName,
		rcr.RequestDate
	FROM RoleChangeRequests rcr
	INNER JOIN Roles r
	ON rcr.RoleId = r.RoleId
	WHERE UserId = @userId
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRequestGetAll]
AS
BEGIN
	SELECT
		rcr.RequestId,
		rcr.UserId,
		r.Name
	AS
		RoleName,
		rcr.RequestDate
	FROM RoleChangeRequests rcr
	INNER JOIN Roles r
	ON rcr.RoleId = r.RoleId
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRequestGetById]
(
	@requestId INT
)
AS
BEGIN
	SELECT
		rcr.RequestId,
		rcr.UserId,
		r.Name
	AS
		RoleName,
		rcr.RequestDate
	FROM RoleChangeRequests rcr
	INNER JOIN Roles r
	ON rcr.RoleId = r.RoleId
	WHERE RequestId = @requestId
END
GO
CREATE PROCEDURE [dbo].[spUserRoleGetAll]
AS
BEGIN
	SELECT Name FROM Roles
END
GO
CREATE PROCEDURE [dbo].[spUserRoleRequestRemove]
(
	@requestId INT
)
AS
BEGIN
	DELETE rcr
	FROM RoleChangeRequests rcr
	WHERE rcr.requestId = @requestId
END
GO

-- IUserSkillStore procedures
IF OBJECT_ID('dbo.spUserSkillGet', 'P') IS NOT NULL
	DROP PROC dbo.spUserSkillGet
IF OBJECT_ID('dbo.spUserSkillSet', 'P') IS NOT NULL
	DROP PROC dbo.spUserSkillSet
IF OBJECT_ID('dbo.spUserSkillClear', 'P') IS NOT NULL
	DROP PROC dbo.spUserSkillClear
IF TYPE_ID('dbo.SkillList') IS NOT NULL
	DROP TYPE dbo.SkillList
GO
CREATE PROCEDURE [dbo].[spUserSkillGet]
(
	@userId INT
)
AS
BEGIN
	SELECT Skills.Name
	FROM Skills
	WHERE Skills.UserId = @userId
END
GO
CREATE TYPE [dbo].[SkillList]
AS TABLE
(
	UserId INT,
	Name VARCHAR(255)
);
GO
CREATE PROCEDURE [dbo].[spUserSkillSet]
(
	@list AS dbo.SkillList READONLY
)
AS
BEGIN
	INSERT INTO Skills (UserId, Name)
	SELECT UserId, Name FROM @list
END
GO
CREATE PROCEDURE [dbo].[spUserSkillClear]
(
	@userId INT
)
AS
BEGIN
	DELETE FROM Skills
	WHERE UserId = @userId
END
GO
