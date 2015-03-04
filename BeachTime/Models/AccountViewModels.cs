using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeachTime.Models
{
	/// <summary>
	/// ViewModel for external login confirmations.
	/// </summary>
    public class ExternalLoginConfirmationViewModel
    {
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

	/// <summary>
	/// ViewModel for listing external logins.
	/// </summary>
    public class ExternalLoginListViewModel
    {
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
        public string Action { get; set; }
		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
        public string ReturnUrl { get; set; }
    }

	/// <summary>
	/// ViewModel for changing a user password.
	/// </summary>
	public class ManageUserViewModel : NavbarViewModelBase
    {
		/// <summary>
		/// Gets or sets the old password.
		/// </summary>
		/// <value>
		/// The old password.
		/// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		/// <value>
		/// The new password.
		/// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>
		/// The confirm password.
		/// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

	/// <summary>
	/// ViewModel for login information.
	/// </summary>
    public class LoginViewModel : NavbarViewModelBase
    {
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required]
		[Display(Name = "Username/Email")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value>
		///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
		/// </value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

	/// <summary>
	/// ViewModel for registering a new user.
	/// </summary>
    public class RegisterViewModel
    {
		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[Required]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[Required]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>
		/// The confirm password.
		/// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

	/// <summary>
	/// ViewModel for resetting a password view email.
	/// </summary>
    public class ResetPasswordViewModel
    {
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>
		/// The confirm password.
		/// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
        public string Code { get; set; }
    }

	/// <summary>
	/// ViewModel for a forgotten password (requesting a reset).
	/// </summary>
    public class ForgotPasswordViewModel
    {
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

	/// <summary>
	/// ViewModel for a role request from a user.
	/// </summary>
	public class RequestRoleViewModel : NavbarViewModelBase
	{
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[Required]
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the role.
		/// </summary>
		/// <value>
		/// The name of the role.
		/// </value>
		[Required]
		public string RoleName { get; set; }

		/// <summary>
		/// Gets or sets the role name list.
		/// </summary>
		/// <value>
		/// The role name list.
		/// </value>
		[Required]
		public List<string> RoleNameList { get; set; }

		/// <summary>
		/// Gets or sets the available roles list.
		/// </summary>
		/// <value>
		/// The available roles list.
		/// </value>
		[Required]
		public List<string> AvailableRolesList { get; set; }

		/// <summary>
		/// Gets or sets the current roles list.
		/// </summary>
		/// <value>
		/// The current roles list.
		/// </value>
		[Required]
		public List<string> CurrentRolesList { get; set; } 
	}

}
