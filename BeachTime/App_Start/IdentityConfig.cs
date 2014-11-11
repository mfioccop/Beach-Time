using System;
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

namespace BeachTime {
	public class BeachUserManager : UserManager<BeachUser> {
		public BeachUserManager(IUserStore<BeachUser> store)
			: base(store) {
		}

		public static BeachUserManager Create(IdentityFactoryOptions<BeachUserManager> options, IOwinContext context) {
			var manager = new BeachUserManager(new UserStore());
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
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null) {
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<BeachUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
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
			var mailMessage = new MailMessage(
				"BeachTime@example.com",
				message.Destination,
				message.Subject,
				message.Body);

			var client = new SmtpClient();
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
