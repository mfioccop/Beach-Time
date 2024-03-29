-- add projects
INSERT INTO Projects (Name, Description, Code, StartDate, EndDate) VALUES ('BeachTime', 'Consultant management system', 'PX123', '20141001', '20150501');
INSERT INTO Projects (Name, Description, Code, StartDate, EndDate) VALUES ('Issue Paper', 'And issue paper', 'PX321', '20141201', '20150201');
INSERT INTO Projects (Name, Description, Code, StartDate, EndDate) VALUES ('Tool Report', 'A tool report', 'PX121', '20141101', '20150302');

-- add users
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('billy.bob@gmail.com', 'Billy', 'Bob', 'billy.bob@gmail.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'ABA8B6l2iak7EMHAz4ij9x5juf0b06FnEbkneLdxltj5Sp5rWI+mGcrmnT4gf1C8nw==', 'd97cbd09-b55c-49f4-890e-9c08488cf400'); -- 'hunter2'
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('jim.bean@yahoo.com', 'Jim', 'Bean', 'jim.bean@yahoo.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'APG2DKJT2/HrvkO4Nv8DURwx9WXzABGQW/ePruIwKKxigRYZrBj+sQCvYmwpE3/GKQ==', 'bdb5a59e-8c69-496a-ad84-93539300b06f'); -- 'password'
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('sarah.smith@aol.com', 'Sarah', 'Smith', 'sarah.smith@aol.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'ADhC7hVfsGVVfkFG3JVSCAQEL0G5g2jSqpwT30PjPtjJW/5n+ICk2D3GOLD2SM1i8g==', '62123d9b-7fe9-4fb6-a543-738c1a21bd36'); -- 'correct horse battery staple'
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('asdf@asdf.asdf', 'asdf', 'fdsa', 'asdf@asdf.asdf', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'AApDVhHUSwNFlL0fN5c78qcmSrVShD6uWOpwbRFDzUxzHZ5jLRTG0/1OI8DKILqMTA==', 'DC1BAFE9-DEAF-487C-951E-D9216B8829F1'); -- 'asdfasdf'

INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('admin', 'Admin', 'Lastly', 'admin@admin.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'ACirJr1HJwvgYfrZK/vDFRC4k7+B/Y+GMzr/jSgE56Kew4mVzdZ/bKbBA0ZSQlCMvA==', '502616cc-e45f-4f0c-b7f0-4f7f1bd26566'); -- '123123'
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp, ProjectId) VALUES ('consultant', 'Consultant', 'Lasting', 'consultant@consultant.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'AF9QLtfkosySl45fGwZ/hpZz6JYanoL2ePBDGD5OS2vaAF1sa1JPvDinbvdjTSewQg==', '01d1a2ed-0c85-404e-a1c5-0b51de9653fd', 1); -- '123123'
INSERT INTO Users (UserName, FirstName, LastName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('executive', 'Executive', 'Lastington', 'executive@executive.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, 'AB7MdX9C2RmIcMvouRnaxPjCGvBAFy+4oC8gIG/yaxtXf8xtljWv3FlwdPGuF/carw==', 'fcffab09-3c6d-4b8f-bfcf-ead281cc4195'); -- '123123'

-- add roles
INSERT INTO Roles (Name) VALUES ('Consultant');
INSERT INTO Roles (Name) VALUES ('Executive');
INSERT INTO Roles (Name) VALUES ('Admin');

-- add skills
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill1');
INSERT INTO Skills (UserId, Name) VALUES (3, 'skill2');
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill3');
INSERT INTO Skills (UserId, Name) VALUES (1, 'skill4');
INSERT INTO Skills (UserId, Name) VALUES (3, 'skill5');

INSERT INTO Skills (UserId, Name) VALUES (6, 'java');
INSERT INTO Skills (UserId, Name) VALUES (6, 'c#');
INSERT INTO Skills (UserId, Name) VALUES (6, 'asp.net');




-- add consultant@consultant.com to Consultant
INSERT INTO UserRoles (UserId, RoleId) VALUES (6, 1);

-- add executive@executive.com to Executive
INSERT INTO UserRoles (UserId, RoleId) VALUES (7, 2);

-- add admin@admin.com to Admin
INSERT INTO UserRoles (UserId, RoleId) VALUES (5, 3);


-- add billy.bob@gmail.com to role1, role3 and role4
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1);
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 3);
--INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 4);

-- add jim.bean@yahoo.com to role2 and role5
INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 2);
--INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 5);

--add external logins
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (1, 'Facebook', 'facebook-key-1');
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (1, 'Google', 'google-key-1');
INSERT INTO ExternalLogins (UserId, LoginProvider, ProviderKey) VALUES (2, 'Google', 'google-key-2');

