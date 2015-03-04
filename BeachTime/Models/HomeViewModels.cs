using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{

	/// <summary>
	/// Abstract ViewModel for other ViewModels to inherit from if a navbar profile section is needed on a page.
	/// </summary>
	public abstract class NavbarViewModelBase
	{
		//[Required]
		/// <summary>
		/// Gets or sets the navbar.
		/// </summary>
		/// <value>
		/// The navbar.
		/// </value>
		public HomeNavbarViewModel Navbar { get; set; }
	}

	/// <summary>
	/// ViewModel for an authenticated user's navbar profile section.
	/// </summary>
	public class HomeNavbarViewModel
	{
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
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Required]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[Required]
		public string Status { get; set; }


	}

	/// <summary>
	/// ViewModel for the pages of the application that only require a navbar profile section and nothing else.
	/// </summary>
	public class HomeViewModel : NavbarViewModelBase
	{
		
	}
}