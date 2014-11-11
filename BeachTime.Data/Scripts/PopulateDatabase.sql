-- add users
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('billy.bob@gmail.com', 'billy.bob@gmail.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, '$s0$14$08$01$64$5sHdwqRaOou+RwHkVkAL/5n+Ia4+QBq5vbmxZoZ5ZyM=$y07Dt63lbKcDvZza7vKpGs3yJ4udRGWauAhDlzXQPw6QcrOt0zOeq0Z3GPvaYFlnfovXF8LZuHxkwWp9bZrEAg==', 'd97cbd09-b55c-49f4-890e-9c08488cf400'); -- 'hunter2'
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('jim.bean@yahoo.com', 'jim.bean@yahoo.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, '$s0$10$08$01$64$8IZQw7JeARPTqmnWr89F6oYGl22XJDfNQX7rKadSnbE=$N+UyzM/Lf0kWOiBH01CAbJ8+2Um8E4Rqo5VpQCV0rKXe8gdywGVl4QRkzNQujTd5kHoRucGQZVfyUNmeulZ3BQ==', 'bdb5a59e-8c69-496a-ad84-93539300b06f'); -- 'password'
INSERT INTO Users (UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc, LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash, SecurityStamp) VALUES ('sarah.smith@aol.com', 'sarah.smith@aol.com', 0, NULL, 0, 0, NULL, 1, 0, 0, NULL, '$s0$15$08$01$64$2AM7TX3uHa8wzvOpsKZkiHSMbNLvCDLBS6QXqRXLGA4=$krI0DyNvI+uIQZnmcA6PR4f/Qv5sFOAU5WqDIaxjucIXjvyoJ0vp983qfNyzj/JjQRkvyQkRlKfzkYY87f8raw==', '62123d9b-7fe9-4fb6-a543-738c1a21bd36'); -- 'correct horse battery staple'
-- add roles
INSERT INTO Roles (Name) VALUES ('role1');
INSERT INTO Roles (Name) VALUES ('role2');
INSERT INTO Roles (Name) VALUES ('role3');
INSERT INTO Roles (Name) VALUES ('role4');
INSERT INTO Roles (Name) VALUES ('role5');
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
