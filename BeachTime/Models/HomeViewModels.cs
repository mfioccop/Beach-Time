using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{

	public abstract class NavbarViewModelBase
	{
		//[Required]
		public HomeNavbarViewModel Navbar { get; set; }
	}

	public class HomeNavbarViewModel
	{
		[Required]
		public string FirstName { get; set; }
		
		[Required]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public int Id { get; set; }

		[Required]
		public string Status { get; set; }


	}

	public class HomeViewModel : NavbarViewModelBase
	{
		
	}
}