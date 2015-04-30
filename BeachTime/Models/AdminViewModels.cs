using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	/// <summary>
	/// ViewModel for the admin index page.
	/// </summary>
	public class AdminIndexViewModel : NavbarViewModelBase
	{
		/// <summary>
		/// Gets or sets the user view models.
		/// </summary>
		/// <value>
		/// The user view models.
		/// </value>
		[Required]
		public List<AdminUserViewModel> UserViewModels { get; set; }

		/// <summary>
		/// Gets or sets the request view models.
		/// </summary>
		/// <value>
		/// The request view models.
		/// </value>
		[Required]
		public List<AdminRoleRequestViewModel> RequestViewModels { get; set; }


		/// <summary>
		/// Gets or sets the register new user view model.
		/// </summary>
		/// <value>
		/// The register new user view model.
		/// </value>
		[Required]
		public RegisterViewModel NewUserViewModel { get; set; }
	}


	/// <summary>
	/// ViewModel for a user's information that an admin can view/modify.
	/// </summary>
	public class AdminUserViewModel
	{
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[Required]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[Required]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[Required]
		public int UserId { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether the user should be deleted.  WARNING: This is a permanent operation that cannot be undone without a database roll-back.
		/// </summary>
		/// <value>
		///   <c>true</c> if [delete user]; otherwise, <c>false</c>.
		/// </value>
		[Required]
		public bool DeleteUser { get; set; }
	}

	/// <summary>
	/// ViewModel for a role request for an admin to view (contains additional user information).
	/// </summary>
	public class AdminRoleRequestViewModel
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
		/// Gets or sets the request identifier.
		/// </summary>
		/// <value>
		/// The request identifier.
		/// </value>
		[Required]
		public int RequestId { get; set; }

		/// <summary>
		/// Gets or sets the request date.
		/// </summary>
		/// <value>
		/// The request date.
		/// </value>
		[Required]
		public DateTime RequestDate { get; set; }
	}

	/// <summary>
	/// ViewModel for the admin's update role page.
	/// </summary>
	public class AdminUpdateRoleViewModel
	{
		/// <summary>
		/// Gets or sets the name of the role.
		/// </summary>
		/// <value>
		/// The name of the role.
		/// </value>
		[Required]
		public string RoleName { get; set; }

		/// <summary>
		/// Gets or sets the request identifier.
		/// </summary>
		/// <value>
		/// The request identifier.
		/// </value>
		[Required]
		public int RequestId { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[Required]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[Required]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[Required]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[Required]
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the request date.
		/// </summary>
		/// <value>
		/// The request date.
		/// </value>
		[Required]
		public DateTime RequestDate { get; set; }
	}

}