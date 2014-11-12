-- add users
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('billy.bob@gmail.com', 'billy.bob@gmail.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'ABA8B6l2iak7EMHAz4ij9x5juf0b06FnEbkneLdxltj5Sp5rWI+mGcrmnT4gf1C8nw==', 'd97cbd09-b55c-49f4-890e-9c08488cf400'); -- 'hunter2'
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('jim.bean@yahoo.com', 'jim.bean@yahoo.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'APG2DKJT2/HrvkO4Nv8DURwx9WXzABGQW/ePruIwKKxigRYZrBj+sQCvYmwpE3/GKQ==', 'bdb5a59e-8c69-496a-ad84-93539300b06f'); -- 'password'
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('sarah.smith@aol.com', 'sarah.smith@aol.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'ADhC7hVfsGVVfkFG3JVSCAQEL0G5g2jSqpwT30PjPtjJW/5n+ICk2D3GOLD2SM1i8g==', '62123d9b-7fe9-4fb6-a543-738c1a21bd36'); -- 'correct horse battery staple'
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('asdf@asdf.asdf', 'asdf@asdf.asdf', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'AApDVhHUSwNFlL0fN5c78qcmSrVShD6uWOpwbRFDzUxzHZ5jLRTG0/1OI8DKILqMTA==', 'DC1BAFE9-DEAF-487C-951E-D9216B8829F1'); -- 'asdfasdf'
-- add roles
INSERT INTO Roles (Name) VALUES ('role1');
INSERT INTO Roles (Name) VALUES ('role2');
INSERT INTO Roles (Name) VALUES ('role3');
INSERT INTO Roles (Name) VALUES ('role4');
INSERT INTO Roles (Name) VALUES ('role5');
-- add skills
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill1');
INSERT INTO Skills (UserId, Name) VALUES (3, 'skill2');
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill3');
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill4');
INSERT INTO Skills (UserId, Name) VALUES (3, 'skill5');
-- add billy.bob@gmail.com to role1, role3 and role4
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1);
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 3);
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 4);
-- add jim.bean@yahoo.com to role2 and role5
INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 2);
INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 5);
--add external logins
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (1, 'Facebook', 'facebook-key-1');
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (1, 'Google', 'google-key-1');
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (2, 'Google', 'google-key-2');
