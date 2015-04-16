using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {

	public class BeachUser : IUser {
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public int AccessFailedCount { get; set; }
		public DateTime? LockoutEndDateUtc { get; set; }
		public bool LockoutEnabled { get; set; }
		public bool EmailTwoFactorEnabled { get; set; }
		public bool GoogleAuthenticatorEnabled { get; set; }
		public string GoogleAuthenticatorSecretKey { get; set; }
		public string PasswordHash { get; set; }
		public Guid SecurityStamp { get; set; }
		public DateTime? LastUpdated { get; set; }

		public string Id { get { return UserId.ToString(CultureInfo.InvariantCulture); } }
		public bool TwoFactorEnabled { get { return EmailTwoFactorEnabled || GoogleAuthenticatorEnabled; } }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<BeachUser> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((BeachUser)obj);
		}

		protected bool Equals(BeachUser other) {
			return UserId == other.UserId &&
			       string.Equals(UserName, other.UserName) &&
				   string.Equals(FirstName, other.FirstName) &&
				   string.Equals(LastName, other.LastName) &&
			       string.Equals(Email, other.Email) &&
			       EmailConfirmed.Equals(other.EmailConfirmed) &&
			       string.Equals(PhoneNumber, other.PhoneNumber) &&
			       PhoneNumberConfirmed.Equals(other.PhoneNumberConfirmed) &&
			       AccessFailedCount == other.AccessFailedCount &&
			       LockoutEndDateUtc.Equals(other.LockoutEndDateUtc) &&
			       LockoutEnabled.Equals(other.LockoutEnabled) &&
			       EmailTwoFactorEnabled.Equals(other.EmailTwoFactorEnabled) &&
			       GoogleAuthenticatorEnabled.Equals(other.GoogleAuthenticatorEnabled) &&
			       string.Equals(GoogleAuthenticatorSecretKey, other.GoogleAuthenticatorSecretKey) &&
			       string.Equals(PasswordHash, other.PasswordHash) &&
			       SecurityStamp.Equals(other.SecurityStamp) &&
				   LastUpdated.Equals(other.LastUpdated);
		}

		public override int GetHashCode() {
			return
				new {
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
					SecurityStamp,
					LastUpdated
				}.GetHashCode();
		}

		public override string ToString() {
			return
				string.Format(
					"UserId: {0}, UserName: {1}, FirstName: {14}, LastName: {15}, Email: {2}, EmailConfirmed: {3}, PhoneNumber: {4}, PhoneNumberConfirmed: {5}, AccessFailedCount: {6}, LockoutEndDateUtc: {7}, LockoutEnabled: {8}, EmailTwoFactorEnabled: {9}, GoogleAuthenticatorEnabled: {10}, GoogleAuthenticatorSecretKey: {11}, PasswordHash: {12}, SecurityStamp: {13}, LastUpdate: {14}",
					UserId, UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEndDateUtc,
					LockoutEnabled, EmailTwoFactorEnabled, GoogleAuthenticatorEnabled, GoogleAuthenticatorSecretKey, PasswordHash,
					SecurityStamp, FirstName, LastName, LastUpdated);
		}
	}

	public class CompareLoginInfo : IEqualityComparer<UserLoginInfo> {
		public bool Equals(UserLoginInfo x, UserLoginInfo y) {
			return (x.LoginProvider == y.LoginProvider) && (x.ProviderKey == y.ProviderKey);
		}

		public int GetHashCode(UserLoginInfo obj) {
			return new { obj.LoginProvider, obj.ProviderKey }.GetHashCode();
		}
	}
}