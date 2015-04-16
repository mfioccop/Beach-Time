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
	public class UserStore : IBeachUserStore, IUserLoginStore<BeachUser>, IUserPasswordStore<BeachUser>,
		IUserSecurityStampStore<BeachUser>, IUserEmailStore<BeachUser>, IUserLockoutStore<BeachUser, string>,
		IUserTwoFactorStore<BeachUser, string>, IBeachUserRoleStore, IUserPhoneNumberStore<BeachUser>,
		IUserSkillStore<BeachUser, string>, IUserBeachStore {
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

		// all user parameters except for @userId
		private static DynamicParameters GetUserParameters(BeachUser beachUser) {
			var p = new DynamicParameters();
			p.Add("@userName", beachUser.UserName);
			p.Add("@firstName", beachUser.FirstName);
			p.Add("@lastName", beachUser.LastName);
			p.Add("@email", beachUser.Email);
			p.Add("@emailConfirmed", beachUser.EmailConfirmed);
			p.Add("@phoneNumber", beachUser.PhoneNumber);
			p.Add("@phoneNumberConfirmed", beachUser.PhoneNumberConfirmed);
			p.Add("@accessFailedCount", beachUser.AccessFailedCount);
			p.Add("@lockoutEndDateUtc", beachUser.LockoutEndDateUtc);
			p.Add("@lockoutEnabled", beachUser.LockoutEnabled);
			p.Add("@emailTwoFactorEnabled", beachUser.EmailTwoFactorEnabled);
			p.Add("@googleAuthenticatorEnabled", beachUser.GoogleAuthenticatorEnabled);
			p.Add("@googleAuthenticatorSecretKey", beachUser.GoogleAuthenticatorSecretKey);
			p.Add("@passwordHash", beachUser.PasswordHash);
			p.Add("@securityStamp", beachUser.SecurityStamp);
			return p;
		}

		#region IUserStore

		public Task CreateAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			var p = GetUserParameters(beachUser);

			using (var con = GetConnection())
				beachUser.UserId = con.Query<int>("spUserCreate", p,
					commandType: CommandType.StoredProcedure).Single();
			return Task.FromResult(0);
		}

		public Task DeleteAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			using (var con = GetConnection())
				con.Execute("spUserDelete", new { beachUser.UserId },
					commandType: CommandType.StoredProcedure);
			return Task.FromResult(0);
		}

		public Task<IEnumerable<BeachUser>> FindAll() {
			using (var con = GetConnection())
				return Task.FromResult(con.Query<BeachUser>("spUserFindAll",
					commandType: CommandType.StoredProcedure));
		}

		public Task<BeachUser> FindByIdAsync(string userId) {
			if (string.IsNullOrEmpty(userId))
				throw new ArgumentNullException("userId");

			using (var con = GetConnection())
				return Task.FromResult(
					con.Query<BeachUser>("spUserFindById", new { userId },
					commandType: CommandType.StoredProcedure).SingleOrDefault());
		}

		public Task<BeachUser> FindByNameAsync(string userName) {
			if (string.IsNullOrEmpty(userName))
				throw new ArgumentNullException("userName");

			using (var con = GetConnection())
				return
					Task.FromResult(
						con.Query<BeachUser>("spUserFindByName", new { userName },
					commandType: CommandType.StoredProcedure).SingleOrDefault());
		}

		public Task UpdateAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			var p = GetUserParameters(beachUser);
			p.Add("@userId", beachUser.UserId);
			p.Add("@lastUpdated", beachUser.LastUpdated);

			using (var con = GetConnection()) {
				var lastUpdated = con.Query<DateTime?>("spUserUpdate", p,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
				// if update fails lastUpdate is null
				if (lastUpdated.HasValue)
					beachUser.LastUpdated = lastUpdated.Value;
			}
			return Task.FromResult(0);
		}

		#endregion IUserStore

		#region IUserLoginStore

		public Task AddLoginAsync(BeachUser beachUser, UserLoginInfo login) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			if (login == null)
				throw new ArgumentNullException("login");

			var p = new DynamicParameters();
			p.Add("@userId", beachUser.UserId);
			p.Add("@loginProvider", login.LoginProvider);
			p.Add("@providerKey", login.ProviderKey);

			using (var con = GetConnection())
				con.Execute("spUserLoginAdd", p,
					commandType: CommandType.StoredProcedure);
			return Task.FromResult(0);
		}

		public Task<BeachUser> FindAsync(UserLoginInfo login) {
			if (login == null)
				throw new ArgumentNullException("login");

			using (var con = GetConnection())
				return Task.FromResult(
					con.Query<BeachUser>("spUserLoginFind", login,
						commandType: CommandType.StoredProcedure).SingleOrDefault());
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(BeachUser beachUser) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			using (var con = GetConnection())
				return Task.FromResult(
					(IList<UserLoginInfo>) con.Query<UserLoginInfo>("spUserLoginGet",
						new { beachUser.UserId },
							commandType: CommandType.StoredProcedure).ToList());
		}

		public Task RemoveLoginAsync(BeachUser beachUser, UserLoginInfo login) {
			if (beachUser == null)
				throw new ArgumentNullException("beachUser");

			if (login == null)
				throw new ArgumentNullException("login");

			var p = new DynamicParameters();
			p.Add("@userId", beachUser.UserId);
			p.Add("@loginProvider", login.LoginProvider);
			p.Add("@providerKey", login.ProviderKey);

			using (var con = GetConnection())
				con.Execute("spUserLoginRemove", p,
					commandType: CommandType.StoredProcedure);
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
					con.Query<BeachUser>("spUserEmailFind", new { email },
						commandType: CommandType.StoredProcedure).SingleOrDefault());
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

			var p = new DynamicParameters();
			p.Add("@userId", user.UserId);
			p.Add("@roleName", roleName);

			using (var con = GetConnection()) {
				con.Execute("spUserRoleAdd", p,
					commandType: CommandType.StoredProcedure);
			}
		}

		public Task<IList<string>> GetRolesAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			using (var con = GetConnection())
				return Task.FromResult((IList<string>)con.Query<string>("spUserRoleGet",
					new { user.UserId }, commandType: CommandType.StoredProcedure).ToList());
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

			var p = new DynamicParameters();
			p.Add("@userId", user.UserId);
			p.Add("@roleName", roleName);

			using (var con = GetConnection())
				con.Execute("spUserRoleRemove", p,
					commandType: CommandType.StoredProcedure);
			return Task.FromResult(0);
		}

		public void RequestRoleChange(RoleChangeRequest request) {
			if (request == null)
				throw new ArgumentNullException("request");

			var p = new DynamicParameters();
			p.Add("@userId", request.UserId);
			p.Add("@roleName", request.RoleName);
			
			using (var con = GetConnection()) {
				var output = con.Query<RoleChangeRequest>("spUserRoleRequestChange", p,
					commandType: CommandType.StoredProcedure).Single();
				request.RequestId = output.RequestId;
				request.RequestDate = output.RequestDate;
			}
		}

		public IEnumerable<RoleChangeRequest> GetRoleChangeRequests(string userId) {
			if (string.IsNullOrEmpty(userId))
				throw new ArgumentNullException("userId");

			using (var con = GetConnection())
				return con.Query<RoleChangeRequest>("spUserRoleRequestGet", new { userId },
					commandType: CommandType.StoredProcedure);
		}

		public IEnumerable<RoleChangeRequest> GetAllRoleChangeRequests()
		{
			using (var con = GetConnection())
				return con.Query<RoleChangeRequest>("spUserRoleRequestGetAll",
					commandType: CommandType.StoredProcedure);
		}
		public IEnumerable<RoleChangeRequest> GetRoleChangeRequestById(string requestId)
		{
			if (string.IsNullOrEmpty(requestId))
				throw new ArgumentNullException("requestId");

			using (var con = GetConnection())
				return con.Query<RoleChangeRequest>("spUserRoleRequestGetById", new { requestId },
					commandType: CommandType.StoredProcedure);
		}

		public IEnumerable<string> GetAllRoles() {
			using (var con = GetConnection())
				return con.Query<string>("spUserRoleGetAll",
					commandType: CommandType.StoredProcedure);
		}

		public Task RemoveRoleRequestAsync(string requestId)
		{
			if (string.IsNullOrEmpty(requestId))
				throw new ArgumentNullException("requestId");

			using (var con = GetConnection())
				con.Execute("spUserRoleRequestRemove", new { requestId },
					commandType: CommandType.StoredProcedure);
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

			using (var con = GetConnection())
				return Task.FromResult((IList<string>)con.Query<string>("spUserSkillGet",
					new { user.UserId }, commandType: CommandType.StoredProcedure).ToList());
		}

		public Task SetSkillsAsync(BeachUser user, IList<string> skills) {
			if (user == null)
				throw new ArgumentNullException("user");
			if (skills == null)
				throw new ArgumentNullException("skills");

			ClearSkillsAsync(user).Wait();

			//var skillsTable = SkillsToDataTable(user.UserId, skills);
			var skillsList = new List<Skill>();
			foreach (var skill in skills) {
				var newSkill = new Skill();
				newSkill.UserId = user.UserId;
				newSkill.Name = skill;
				skillsList.Add(newSkill);
			}

			var param = new SkillDynamicParam(skillsList);

			using (var con = GetConnection())
				con.Execute("spUserSkillSet", param,
					commandType: CommandType.StoredProcedure);

			return Task.FromResult(0);
		}

		public Task ClearSkillsAsync(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			using (var con = GetConnection())
				con.Execute("spUserSkillClear", new { user.UserId },
						commandType: CommandType.StoredProcedure);

			return Task.FromResult(0);
		}


		#endregion IUserSkillStore

		#region IUserBeachStore

		public bool OnBeach(BeachUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			using (var con = GetConnection())
				return con.Query<bool>("spUserBeachOn", new { user.UserId },
					commandType: CommandType.StoredProcedure).Single();
		}

		public IEnumerable<BeachUser> GetBeachedUsers() {
			using (var con = GetConnection())
				return con.Query<BeachUser>("spUserBeachGet",
					commandType: CommandType.StoredProcedure);
		}

		#endregion IUserBeachStore

		#region Helpers
		struct Skill {
			public int UserId;
			public string Name;
		}

		class SkillDynamicParam : Dapper.SqlMapper.IDynamicParameters {
			IEnumerable<Skill> skills;
			public SkillDynamicParam(IEnumerable<Skill> skills) {
				this.skills = skills;
			}

			public void AddParameters(IDbCommand command, SqlMapper.Identity identity) {
				var sqlCommand = (SqlCommand)command;
				sqlCommand.CommandType = CommandType.StoredProcedure;

				var skillList = new List<Microsoft.SqlServer.Server.SqlDataRecord>();

				// Create an SqlMetaData object that describes our table type.
				Microsoft.SqlServer.Server.SqlMetaData[] tvpDefinition = {
					new Microsoft.SqlServer.Server.SqlMetaData("UserId", SqlDbType.Int),
					new Microsoft.SqlServer.Server.SqlMetaData("Name", SqlDbType.VarChar, 255)
				};

				foreach (var skill in skills) {
					// Create a new record, using the metadata array above.
					var rec = new Microsoft.SqlServer.Server.SqlDataRecord(tvpDefinition);
					rec.SetInt32(0, skill.UserId);    // Set the value.
					rec.SetString(1, skill.Name);
					skillList.Add(rec);      // Add it to the list.
				}

				// Add the table parameter.
				var p = sqlCommand.Parameters.Add("@list", SqlDbType.Structured);
				p.Direction = ParameterDirection.Input;
				p.TypeName = "SkillList";
				p.Value = skillList;

			}
		}
		#endregion Helpers
	}
}
