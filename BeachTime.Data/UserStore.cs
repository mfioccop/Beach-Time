using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
	public class UserStore : IUserStore<BeachUser>, IUserLoginStore<BeachUser>, IUserPasswordStore<BeachUser>,
		IUserSecurityStampStore<BeachUser>, IUserEmailStore<BeachUser>, IUserLockoutStore<BeachUser, string>,
		IUserTwoFactorStore<BeachUser, string>, IUserRoleStore<BeachUser>, IUserPhoneNumberStore<BeachUser>,
		IUserSkillStore<BeachUser, string> {
		private readonly string connectionString;

		public UserStore(string connectionStringName) {
			if (string.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException("connectionStringName");

			connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		public UserStore()
			: this("DefaultConnection") {
		}

		private IDbConnection GetConnection() {
			IDbConnection con = new SqlConnection(connectionString);
			con.Open();
			return con;
		}

		public void Dispose() {
		}

		#region IUserStore

		public Task CreateAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			const string createQuery =
				@"insert into Users (
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
) output Inserted.UserId values (
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
	@securityStamp)";

			using (var con = GetConnection())
				beachUser.UserId = con.Query<int>(createQuery, beachUser).Single();
			return Task.FromResult(0);
		}

		public Task DeleteAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			using (var con = GetConnection())
				con.Execute("delete from Users where UserId = @userId", new { beachUser.UserId });
			return Task.FromResult(0);
		}

		public Task<BeachUser> FindByIdAsync(string userId) {
			if (string.IsNullOrEmpty(userId))
				throw new ArgumentNullException("userId");

			using (var con = GetConnection())
				return Task.FromResult(
					con.Query<BeachUser>("select * from Users where UserId = @userId", new { userId }).SingleOrDefault());
		}

		public Task<BeachUser> FindByNameAsync(string userName) {
			if (string.IsNullOrEmpty(userName))
				throw new ArgumentNullException("userName");

			using (var con = GetConnection())
				return
					Task.FromResult(
						con.Query<BeachUser>("select * from Users where UserName = @userName", new { userName }).SingleOrDefault());
		}

		public Task UpdateAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			const string updateQuery =
				@"update Users set
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
where UserId = @userId";

			using (var con = GetConnection())
				con.Execute(updateQuery, beachUser);
			return Task.FromResult(0);
		}

		#endregion IUserStore

		#region IUserLoginStore

		public Task AddLoginAsync(BeachUser beachUser, UserLoginInfo login) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			if (login == null)
				throw new ArgumentNullException("login");

			using (var con = GetConnection())
				con.Execute(
					"insert into ExternalLogins(UserId, LoginProvider, ProviderKey) values (@userId, @loginProvider, @providerKey)",
					new {
						userId = beachUser.UserId,
						loginProvider = login.LoginProvider,
						providerKey = login.ProviderKey
					});
			return Task.FromResult(0);
		}

		public Task<BeachUser> FindAsync(UserLoginInfo login) {
			if (login == null)
				throw new ArgumentNullException("login");

			using (var con = GetConnection())
				return Task.FromResult(
					con.Query<BeachUser>(
						"select u.* from Users u inner join ExternalLogins l on l.UserId = u.UserId where l.LoginProvider = @loginProvider and l.ProviderKey = @providerKey",
						login).SingleOrDefault());
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			using (var con = GetConnection())
				return
					Task.FromResult(
						(IList<UserLoginInfo>)
							con.Query<UserLoginInfo>("select LoginProvider, ProviderKey from ExternalLogins where UserId = @userId",
								new { beachUser.UserId }).ToList());
		}

		public Task RemoveLoginAsync(BeachUser beachUser, UserLoginInfo login) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			if (login == null)
				throw new ArgumentNullException("login");

			using (var con = GetConnection())
				con.Execute(
					"delete from ExternalLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey",
					new {
						beachUser.UserId,
						login.LoginProvider,
						login.ProviderKey
					});
			return Task.FromResult(0);
		}

		#endregion IUserLoginStore

		#region IUserPasswordStore

		public Task<string> GetPasswordHashAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			return Task.FromResult(beachUser.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			return Task.FromResult(!string.IsNullOrEmpty(beachUser.PasswordHash));
		}

		public Task SetPasswordHashAsync(BeachUser beachUser, string passwordHash) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			if (string.IsNullOrEmpty(passwordHash))
				throw new ArgumentNullException("passwordHash");

			beachUser.PasswordHash = passwordHash;

			return Task.FromResult(0);
		}

		#endregion IUserPasswordStore

		#region IUserSecurityStampStore

		public Task<string> GetSecurityStampAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			return Task.FromResult(beachUser.SecurityStamp.ToString());
		}

		public Task SetSecurityStampAsync(BeachUser beachUser, string stamp) {
			if (beachUser == null)
				throw new ArgumentNullException("BeachUser");

			if (string.IsNullOrEmpty(stamp))
				throw new ArgumentNullException("stamp");

			beachUser.SecurityStamp = Guid.Parse(stamp);

			return Task.FromResult(0);
		}

		#endregion IUserSecurityStampStore

		#region IUserEmailStore

		public Task<BeachUser> FindByEmailAsync(string email) {
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");

			using (var con = GetConnection())
				return Task.FromResult(
					con.Query<BeachUser>(
						"select * from Users where Email = @email", new { email }).SingleOrDefault());
		}

		public Task<string> GetEmailAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.EmailConfirmed);
		}

		public Task SetEmailAsync(BeachUser user, string email) {
			if (user == null)
				throw new ArgumentNullException("user");

			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");

			user.Email = email;
			return Task.FromResult(0);
		}

		public Task SetEmailConfirmedAsync(BeachUser user, bool confirmed) {
			if (user == null)
				throw new ArgumentNullException("user");

			user.EmailConfirmed = confirmed;
			return Task.FromResult(0);
		}

		#endregion IUserEmailStore

		#region IUserLockoutStore

		public Task<int> GetAccessFailedCountAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.AccessFailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.LockoutEnabled);
		}

		public Task<DateTimeOffset> GetLockoutEndDateAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(
				user.LockoutEndDateUtc.HasValue
					? new DateTimeOffset(user.LockoutEndDateUtc.Value.ToLocalTime())
					: DateTimeOffset.MinValue);
		}

		public Task<int> IncrementAccessFailedCountAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(++user.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			user.AccessFailedCount = 0;
			return Task.FromResult(0);
		}

		public Task SetLockoutEnabledAsync(BeachUser user, bool enabled) {
			if (user == null)
				throw new ArgumentNullException("user");

			user.LockoutEnabled = enabled;
			return Task.FromResult(0);
		}

		public Task SetLockoutEndDateAsync(BeachUser user, DateTimeOffset lockoutEnd) {
			if (user == null)
				throw new ArgumentNullException("user");

			user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
			return Task.FromResult(0);
		}

		#endregion IUserLockoutStore

		#region IUserTwoFactorStore

		public Task<bool> GetTwoFactorEnabledAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.TwoFactorEnabled);
		}

		public Task SetTwoFactorEnabledAsync(BeachUser user, bool enabled) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (enabled && !string.IsNullOrWhiteSpace(user.Email) && user.EmailConfirmed)
				user.EmailTwoFactorEnabled = true;
			else if (!enabled)
				user.EmailTwoFactorEnabled = false;

			return Task.FromResult(0);
		}

		#endregion IUserTwoFactorStore

		#region IUserRoleStore

		public async Task AddToRoleAsync(BeachUser user, string roleName) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (string.IsNullOrEmpty(roleName))
				throw new ArgumentNullException("roleName");

			//maybe just return?
			if (await IsInRoleAsync(user, roleName))
				throw new InvalidOperationException(user.UserName + " is already in role " + roleName);

			using (var con = GetConnection()) {
				con.Execute(
					"insert into UserRoles (UserId, RoleId) select distinct @userId as UserId, RoleId from Users, Roles where Roles.Name = @roleName",
					new { user.UserId, roleName });
			}
		}

		public Task<IList<string>> GetRolesAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			const string getRolesQuery =
				@"select Roles.Name
from Roles
join UserRoles on UserRoles.RoleId = Roles.RoleId
where UserRoles.UserId = @userId";
			using (var con = GetConnection())
				return Task.FromResult((IList<string>)con.Query<string>(getRolesQuery, new { user.UserId }).ToList());
		}

		public Task<bool> IsInRoleAsync(BeachUser user, string roleName) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (string.IsNullOrEmpty(roleName))
				throw new ArgumentNullException("roleName");

			return Task.FromResult(GetRolesAsync(user).Result.Contains(roleName));
		}

		public Task RemoveFromRoleAsync(BeachUser user, string roleName) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (string.IsNullOrEmpty(roleName))
				throw new ArgumentNullException("roleName");

			const string removeFromRolesQuery =
				@"delete ur
from UserRoles ur
join Roles r
	on r.RoleId = ur.RoleId
	and ur.UserId = @userId
	and r.Name = @roleName";
			using (var con = GetConnection())
				con.Execute(removeFromRolesQuery, new { user.UserId, roleName });
			return Task.FromResult(0);
		}

		#endregion IUserRoleStore

		#region IUserPhoneNumberStore

		public Task<string> GetPhoneNumberAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.PhoneNumber);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.PhoneNumberConfirmed);
		}

		public Task SetPhoneNumberAsync(BeachUser user, string phoneNumber) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (string.IsNullOrEmpty(phoneNumber))
				throw new ArgumentNullException("phoneNumber");

			user.PhoneNumber = phoneNumber;
			return Task.FromResult(0);
		}

		public Task SetPhoneNumberConfirmedAsync(BeachUser user, bool confirmed) {
			if (user == null)
				throw new ArgumentNullException("user");

			user.PhoneNumberConfirmed = confirmed;
			return Task.FromResult(0);
		}

		#endregion IUserPhoneNumberStore

		#region IUserSkillStore

		public Task<IList<string>> GetSkillsAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			const string getSkillsQuery =
				@"select Skills.Name
from Skills
where Skills.UserId = @userId";
			using (var con = GetConnection())
				return Task.FromResult((IList<string>)con.Query<string>(getSkillsQuery, new { user.UserId }).ToList());
		}

		private struct Skill {
			public string UserId { get; set; }
			public string Name { get; set; }
		}

		public Task SetSkillsAsync(BeachUser user, IList<string> skills) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (skills == null)
				throw new ArgumentNullException("skills");

			var userSkills = new List<Skill>();
			foreach (var skill in skills)
				userSkills.Add(new Skill{UserId = user.Id, Name = skill});

			ClearSkillsAsync(user).Wait();

			using (var con = GetConnection())
				con.Execute("insert into Skills values (@userId, @name)", userSkills);

			return Task.FromResult(0);
		}

		public Task ClearSkillsAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			using (var con = GetConnection())
				con.Execute("delete from Skills where UserId = @userId", new {user.UserId});

			return Task.FromResult(0);
		}


		#endregion IUserSkillStore
	}
}
