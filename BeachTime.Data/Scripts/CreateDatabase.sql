-- drop existing tables if they exist
IF OBJECT_ID('dbo.ExternalLogins', 'U') IS NOT NULL
	DROP TABLE dbo.ExternalLogins;
IF OBJECT_ID('dbo.UserRoles', 'U') IS NOT NULL
	DROP TABLE dbo.UserRoles;
IF OBJECT_ID('dbo.Skills', 'U') IS NOT NULL
	DROP TABLE dbo.Skills;
IF OBJECT_ID('dbo.FileInfo', 'U') IS NOT NULL
	DROP TABLE dbo.FileInfo;
IF OBJECT_ID('dbo.Projects', 'U') IS NOT NULL
	DROP TABLE dbo.Projects;
IF OBJECT_ID('dbo.RoleChangeRequests', 'U') IS NOT NULL
	DROP TABLE dbo.RoleChangeRequests;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
	DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL
	DROP TABLE dbo.Roles;

-- create tables
CREATE TABLE [dbo].[Users]
(
	[UserId] INT IDENTITY NOT NULL,
	[UserName] VARCHAR(255) NOT NULL,
	[FirstName] VARCHAR(255) NULL,
	[LastName] VARCHAR(255) NULL,
	[Email] VARCHAR(255) NOT NULL,
	[EmailConfirmed] BIT NOT NULL,
	[PhoneNumber] VARCHAR(15) NULL,
	[PhoneNumberConfirmed] BIT NOT NULL,
	[AccessFailedCount] INT NOT NULL,
	[LockoutEndDateUtc] DATETIME2 NULL,
	[LockoutEnabled] BIT NOT NULL,
	[EmailTwoFactorEnabled] BIT NOT NULL,
	[GoogleAuthenticatorEnabled] BIT NOT NULL,
	[GoogleAuthenticatorSecretKey] VARCHAR(32) NULL,
	[PasswordHash] VARCHAR(149) NULL,
	[SecurityStamp] UNIQUEIDENTIFIER NULL,
	CONSTRAINT PK_Users PRIMARY KEY ([UserId]),
	CONSTRAINT CK_Phone_Number CHECK ([PhoneNumber] IS NULL OR (LEFT([PhoneNumber], 1) LIKE '[0-9]' AND TRY_CAST([PhoneNumber] AS bigint) IS NOT NULL))
);

CREATE TABLE [dbo].[ExternalLogins]
(
	[UserId] INT NOT NULL,
	[LoginProvider] VARCHAR(255) NOT NULL,
	[ProviderKey] VARCHAR(255) NOT NULL,
	CONSTRAINT [PK_ExternalLogins] PRIMARY KEY
	(
		[UserId],
		[LoginProvider],
		[ProviderKey]
	),
	CONSTRAINT [FK_ExternalLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);
CREATE INDEX IX_ExternalLogins_UserId ON ExternalLogins([UserId]);

CREATE TABLE [dbo].[Roles]
(
	[RoleId] INT IDENTITY NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
);
CREATE UNIQUE INDEX IX_Roles_Name ON Roles(Name);

CREATE TABLE [dbo].[Skills]
(
	[UserId] INT NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	CONSTRAINT [PK_Skills] PRIMARY KEY
	(
		[UserId],
		[Name]
	),
	CONSTRAINT [FK_Skills_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);
CREATE INDEX IX_Skills_Name ON Skills(Name);

CREATE TABLE [dbo].[Projects]
(
	[ProjectId] INT IDENTITY NOT NULL,
	[UserId] INT NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[Completed] BIT NOT NULL,
	CONSTRAINT [PK_Projects] PRIMARY KEY ([ProjectId]),
	CONSTRAINT [FK_Projects_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);
CREATE UNIQUE INDEX IX_Projects_Name ON Projects(Name);

CREATE TABLE [dbo].[FileInfo]
(
	[FileId] INT IDENTITY NOT NULL,
	[UserId] INT NOT NULL,
	[Path] VARCHAR(900) NOT NULL,
	[Title] VARCHAR(255) NOT NULL,
	[Description] VARCHAR(8000) NOT NULL,
	CONSTRAINT [PK_FileInfo] PRIMARY KEY ([FileId]),
	CONSTRAINT [FK_FileInfo_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE,
	CONSTRAINT [AK_FileInfo_Path] UNIQUE ([Path])
);
CREATE UNIQUE INDEX IX_FileInfo_Title ON FileInfo(Title);

CREATE TABLE [dbo].[UserRoles]
(
	[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL,
	CONSTRAINT [PK_UserRoles] PRIMARY KEY
	(
		[UserId],
		[RoleId]
	),
	CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([RoleId]) ON DELETE CASCADE
);
CREATE INDEX IX_UserRoles_UserId ON UserRoles(UserId);
CREATE INDEX IX_UserRoles_RoleId ON UserRoles(RoleId);

CREATE TABLE [dbo].[RoleChangeRequests]
(
	[RequestId] INT IDENTITY NOT NULL,
	[RoleId] INT NOT NULL,
	[UserId] INT NOT NULL,
	[RequestDate] DATETIME2 NOT NULL,
	CONSTRAINT [PK_RoleChangeRequest] PRIMARY KEY ([RequestId]),
	CONSTRAINT [FK_RoleChangeRequests_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE,
	CONSTRAINT [FK_RoleChangeRequests_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([RoleId]) ON DELETE CASCADE
);
CREATE INDEX IX_RoleChangeRequests_UserId ON RoleChangeRequests(UserId);
CREATE INDEX IX_RoleChangeRequests_RoleId ON RoleChangeRequests(RoleId);
