using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class AdminIndexViewModel : NavbarViewModelBase
	{
		[Required]
		public List<AdminUserViewModel> UserViewModels { get; set; }

		[Required]
		public List<AdminRoleRequestViewModel> RequestViewModels { get; set; }
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
		public int UserId { get; set; }
	}
	
	public class AdminRoleRequestViewModel
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		public string RoleName { get; set; }

		[Required]
		public int RequestId { get; set; }

		[Required]
		public DateTime RequestDate { get; set; }
	}

	public class AdminUpdateRoleViewModel
	{
		[Required]
		public string RoleName { get; set; }

		[Required]
		public int RequestId { get; set; }

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
		public int UserId { get; set; }

		[Required]
		public DateTime RequestDate { get; set; }
	}

}