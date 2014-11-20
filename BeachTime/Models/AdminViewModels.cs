using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class AdminIndexViewModel
	{
		public List<UserViewModel> UserViewModels { get; set; }
	}

	public class UserViewModel
	{
		public string UserName { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		[EmailAddress]
		public string Email { get; set; }

	}
}