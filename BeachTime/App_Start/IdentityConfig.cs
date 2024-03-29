﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using BeachTime.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace BeachTime {
	public class BeachUserManager : UserManager<BeachUser> {
		public BeachUserManager(IUserStore<BeachUser> store)
			: base(store) {
		}

		public static BeachUserManager Create(IdentityFactoryOptions<BeachUserManager> options, IOwinContext context) {
			BeachUserManager manager = new BeachUserManager(new UserStore());
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<BeachUser>(manager) {
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			//TODO re-enable password strength enforcements, disabled for development
			manager.PasswordValidator = new PasswordValidator {
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false,
			};

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null) {
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<BeachUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}

		public IEnumerable<BeachUser> FindAll() {
			IBeachUserStore userStore = (IBeachUserStore)Store;
			return userStore.FindAll().Result;
		} 
		
		public IList<string> GetUserSkills(BeachUser user) {
			IUserSkillStore<BeachUser, string> skillStore = (IUserSkillStore<BeachUser, string>)Store;
			return skillStore.GetSkillsAsync(user).Result;
		}

		public void SetUserSkills(BeachUser user, IList<string> skills) {
			IUserSkillStore<BeachUser, string> skillStore = (IUserSkillStore<BeachUser, string>)Store;
			skillStore.SetSkillsAsync(user, skills).Wait();
		}

		public void ClearUserSkills(BeachUser user) {
			IUserSkillStore<BeachUser, string> skillStore = (IUserSkillStore<BeachUser, string>)Store;
			skillStore.ClearSkillsAsync(user).Wait();
		}

		public bool UserOnBeach(BeachUser user) {
			IUserBeachStore beachStore = (IUserBeachStore)Store;
			return beachStore.OnBeach(user);
		}

		public IEnumerable<BeachUser> GetBeachedUsers() {
			IUserBeachStore beachStore = (IUserBeachStore)Store;
			return beachStore.GetBeachedUsers();
		}

		public void RequestRoleChange(RoleChangeRequest request) {
			IBeachUserRoleStore roleStore = (IBeachUserRoleStore)Store;
			roleStore.RequestRoleChange(request);
		}

		public IEnumerable<RoleChangeRequest> GetRoleChangeRequests(string userId) {
			IBeachUserRoleStore roleStore = (IBeachUserRoleStore)Store;
			return roleStore.GetRoleChangeRequests(userId);
		}
	}

	public class BeachSignInManager : SignInManager<BeachUser, string> {
		public BeachSignInManager(BeachUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager) {
		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(BeachUser user) {
			return user.GenerateUserIdentityAsync((BeachUserManager)UserManager);
		}

		public static BeachSignInManager Create(IdentityFactoryOptions<BeachSignInManager> options, IOwinContext context) {
			return new BeachSignInManager(context.GetUserManager<BeachUserManager>(), context.Authentication);
		}
	}

	public class EmailService : IIdentityMessageService {
		public Task SendAsync(IdentityMessage message) {
			MailMessage mailMessage = new MailMessage(
				"BeachTime@example.com",
				message.Destination,
				message.Subject,
				message.Body);

			SmtpClient client = new SmtpClient();
			client.SendAsync(mailMessage, null);

			return Task.FromResult(0);
		}
	}

	public class SmsService : IIdentityMessageService {
		public Task SendAsync(IdentityMessage message) {
			// Plug in your SMS service here to send a text message.
			return Task.FromResult(0);
		}
	}
}
