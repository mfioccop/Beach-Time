using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class AdminIndexViewModel
	{
		public List<AdminUserViewModel> UserViewModels { get; set; }
	}

	public class AdminUserViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public int Id { get; set; }
	}
}