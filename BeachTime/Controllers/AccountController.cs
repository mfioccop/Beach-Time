using System.Net;
using BeachTime.Data;

namespace BeachTime.Controllers
{
	using BeachTime.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin.Security;
	using Owin;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using System.Web;
	using System.Web.Mvc;

	/// <summary>
	/// AccountController class handles registration, login, logout of user accounts using the Identity Framework
	/// and external authentication methods
	/// </summary>
	[Authorize]
	public class AccountController : Controller
	{
		#region UserManagerConstructor

		/// <summary>
		/// Manages user account registration and authentication
		/// </summary>
		private BeachUserManager _userManager;

		/// <summary>
		/// Initializes a new instance of the AccountController class
		/// </summary>
		public AccountController()
		{
		}

		/// <summary>
		/// Initializes a new instance of the AccountController class
		/// </summary>
		/// <param name="userManager">BeachUserManager that the application should use</param>
		public AccountController(BeachUserManager userManager)
		{
			UserManager = userManager;
		}

		/// <summary>
		/// Gets the current UserManager
		/// </summary>
		public BeachUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BeachUserManager>(); }

			private set { _userManager = value; }

		}

		#endregion

		#region Login

		/// <summary>
		/// GET: /Account/Login
		/// </summary>
		/// <param name="returnUrl">URL to redirect the user to when the login process is complete</param>
		/// <returns>The /Account/Login view</returns>
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View(new LoginViewModel()
			{
				Navbar = new HomeNavbarViewModel()
			});
		}

		/// <summary>
		/// POST: /Account/Login
		/// </summary>
		/// <param name="model">Login View Model</param>
		/// <param name="returnUrl">URL to redirect the user to when the login process is complete</param>
		/// <returns>If login was successful: redirect to returnUrl, otherwise reload the view with errors displayed</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				// First search for a user by username/password
				var user = await UserManager.FindAsync(model.UserName, model.Password);
				if (user != null)
				{
					await SignInAsync(user, model.RememberMe);
					return RedirectToLocal(returnUrl);
				}
				else
				{
					// If that fails, then search for a user with the given email address
					var userByEmail = await UserManager.FindByEmailAsync(model.UserName);

					if (userByEmail != null)
					{
						// Now double check that the given password matches (otherwise all the would be needed to login is an email address)
						user = await UserManager.FindAsync(userByEmail.UserName, model.Password);

						if (user == null) return View(model);

						await SignInAsync(user, model.RememberMe);
						return RedirectToLocal(returnUrl);
					}
					else
					{
						// Both login attempts failed, invalid info
						ModelState.AddModelError("", "Invalid username or password.");
					}

				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion

		#region Register

		/// <summary>
		/// GET: /Account/Register
		/// </summary>
		/// <returns>The /Account/Register view</returns>
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		/// <summary>
		/// POST: /Account/Register
		/// </summary>
		/// <param name="model">Register View Model</param>
		/// <returns>Redirects to /Home/Index if registration was successful, or reloads the View if it was not</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new BeachUser()
				{
					UserName = model.UserName,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName
				};
				IdentityResult result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await SignInAsync(user, isPersistent: false);


					UserManager.AddToRole(user.Id, "Consultant");

					//UserManager.AddToRole(User.Identity.GetUserId(), "Consultant");
					// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
					// Send an email with this link
					//// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
					//// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
					//// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

					return RedirectToAction("Index", "Home");
				}
				else
				{
					AddErrors(result);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion

		#region EmailPassword

		/// <summary>
		/// GET: /Account/ConfirmEmail
		/// </summary>
		/// <param name="userId">User's id that needs to have its email confirmed</param>
		/// <param name="code">Confirmation code to compare to the Identity framework's generated code</param>
		/// <returns>The Error view if the userId or code is null, or reloads the view if successful</returns>
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}

			IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
			if (result.Succeeded)
			{
				return View("ConfirmEmail");
			}
			else
			{
				AddErrors(result);
				return View();
			}
		}

		/// <summary>
		/// GET: /Account/ForgotPassword
		/// </summary>
		/// <returns>The /Account/ForgotPassword view</returns>
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		/// <summary>
		/// POST: /Account/ForgotPassword
		/// </summary>
		/// <param name="model">Forgot Password View Model</param>
		/// <returns>Sends an email to the user if they forgot their password, prompting them to reset it, or if the user does
		/// not exist then display appropriate error</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
				{
					ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
					return View();
				}

				// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
				// Send an email with this link
				// string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
				// var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
				// await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
				// return RedirectToAction("ForgotPasswordConfirmation", "Account");
			}

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/ForgotPasswordConfirmation
		/// </summary>
		/// <returns>The /Account/ForgotPasswordConfirmation view</returns>
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return this.View();
		}

		/// <summary>
		/// GET: /Account/ResetPassword
		/// </summary>
		/// <param name="code">The code to reset the password for a user's account</param>
		/// <returns>An Error view if the code is not placed in the URL, or displays the /Account/ResetPassword page if the code is present</returns>
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			if (code == null)
			{
				return View("Error");
			}

			return this.View();
		}

		/// <summary>
		/// POST: /Account/ResetPassword
		/// </summary>
		/// <param name="model">Reset Password View Model</param>
		/// <returns>If the password reset was successful: redirect to /Account/ResetPasswordConfirmation, 
		/// otherwise reload the view with errors displayed</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "No user found.");
					return this.View();
				}

				IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
				if (result.Succeeded)
				{
					return this.RedirectToAction("ResetPasswordConfirmation", "Account");
				}

				else
				{
					this.AddErrors(result);
					return this.View();
				}
			}

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/ResetPasswordConfirmation
		/// </summary>
		/// <returns>The /Account/ResetPasswordConfirmation view</returns>
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return this.View();
		}

		#endregion

		#region Manage

		/// <summary>
		/// GET: /Account/Manage
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess
					? "Your password has been changed."
					: message == ManageMessageId.SetPasswordSuccess
						? "Your password has been set."
						: message == ManageMessageId.RemoveLoginSuccess
							? "The external login was removed."
							: message == ManageMessageId.Error
								? "An error has occurred."
								: "";
			ViewBag.HasLocalPassword = this.HasPassword();
			ViewBag.ReturnUrl = Url.Action("Manage");
			return this.View();
		}

		/// <summary>
		/// POST: /Account/Manage
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Manage(ManageUserViewModel model)
		{
			bool hasPassword = this.HasPassword();
			ViewBag.HasLocalPassword = hasPassword;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasPassword)
			{
				if (ModelState.IsValid)
				{
					IdentityResult result =
						await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
					if (result.Succeeded)
					{
						var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
						await this.SignInAsync(user, isPersistent: false);
						return this.RedirectToAction("Manage", new {Message = ManageMessageId.ChangePasswordSuccess});
					}
					else
					{
						this.AddErrors(result);
					}
				}
			}
			else
			{
				// User does not have a password so remove any validation errors caused by a missing OldPassword field
				ModelState state = ModelState["OldPassword"];
				if (state != null)
				{
					state.Errors.Clear();
				}

				if (ModelState.IsValid)
				{
					IdentityResult result = await this.UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
					if (result.Succeeded)
					{
						return this.RedirectToAction("Manage", new {Message = ManageMessageId.SetPasswordSuccess});
					}
					else
					{
						this.AddErrors(result);
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return this.View(model);
		}

		#endregion

		#region ExternalLogin

		/// <summary>
		/// POST: /Account/Disassociate
		/// </summary>
		/// <param name="loginProvider"></param>
		/// <param name="providerKey"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
		{
			ManageMessageId? message = null;
			IdentityResult result =
				await this.UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
				await this.SignInAsync(user, isPersistent: false);
				message = ManageMessageId.RemoveLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}
			return this.RedirectToAction("Manage", new {Message = message});
		}

		/// <summary>
		/// POST: /Account/ExternalLogin
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="returnUrl">URL to redirect the user to when the login process is complete</param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
		}

		/// <summary>
		/// GET: /Account/ExternalLoginCallback
		/// </summary>
		/// <param name="returnUrl">URL to redirect the user to when the login process is complete</param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return this.RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var user = await this.UserManager.FindAsync(loginInfo.Login);
			if (user != null)
			{
				await this.SignInAsync(user, isPersistent: false);
				return this.RedirectToLocal(returnUrl);
			}
			else
			{
				// If the user does not have an account, then prompt the user to create an account
				ViewBag.ReturnUrl = returnUrl;
				ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
				return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
			}
		}

		/// <summary>
		/// POST: /Account/LinkLogin
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
		}

		/// <summary>
		/// GET: /Account/LinkLoginCallback
		/// </summary>
		/// <returns></returns>
		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null)
			{
				return this.RedirectToAction("Manage", new {Message = ManageMessageId.Error});
			}

			IdentityResult result = await this.UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			if (result.Succeeded)
			{
				return this.RedirectToAction("Manage");
			}

			return this.RedirectToAction("Manage", new {Message = ManageMessageId.Error});
		}

		/// <summary>
		/// POST: /Account/ExternalLoginConfirmation
		/// </summary>
		/// <param name="model"></param>
		/// <param name="returnUrl">URL to redirect the user to when the login process is complete</param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Manage");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return this.View("ExternalLoginFailure");
				}

				var user = new BeachUser() {UserName = model.Email, Email = model.Email};
				IdentityResult result = await this.UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await this.SignInAsync(user, isPersistent: false);

						// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
						// Send an email with this link
						//// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
						//// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
						//// SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

						return this.RedirectToLocal(returnUrl);
					}

				}

				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		/// <summary>
		/// POST: /Account/LogOff
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// GET: /Account/ExternalLoginFailure
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return this.View();
		}

		/// <summary>
		/// Remove account list
		/// </summary>
		/// <returns></returns>
		[ChildActionOnly]
		public ActionResult RemoveAccountList()
		{
			var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
			ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
			return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Dispose method override for IDisposable interface
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && UserManager != null)
			{
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(BeachUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		private void SendEmail(string email, string callbackUrl, string subject, string message)
		{
			// For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }

			public string RedirectUri { get; set; }

			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion

		#region RequestRole

		// GET: Account/RequestRole
		public ActionResult RequestRole()
		{
			try
			{
				if (User.Identity.GetUserId() == null)
					HttpContext.AddError(new HttpException(403, "You need to be logged in to view this page."));


				var store = new UserStore();
				var user = store.FindByIdAsync(User.Identity.GetUserId()).Result;
				// Get all current requests 
				var requests = store.GetRoleChangeRequests(User.Identity.GetUserId());
				var pending = requests.Select(request => request.RoleName);
				var roles = store.GetAllRoles().ToList();

				var requestViewModel = new RequestRoleViewModel()
				{
					RoleName = String.Empty,
					UserId = user.UserId,
					RoleNameList = roles,
					AvailableRolesList = roles.Except(pending).ToList(),
					CurrentRolesList = store.GetRolesAsync(user).Result.ToList(),
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
						Id = user.UserId,
						Status = String.Empty
					}
				};

				return View(requestViewModel);
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		// POST: Account/RequestRole
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RequestRole(RequestRoleViewModel model)
		{
			try
			{
				var request = new RoleChangeRequest()
				{
					UserId = model.UserId,
					RoleName = model.RoleName
				};

				UserManager.RequestRoleChange(request);
			}
			catch
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("RequestRole", "Account");
		}

		#endregion

	}
}