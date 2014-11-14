using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class ConsultantIndexViewModel
	{
		[DisplayName("First Name")]
		public string FirstName { get; set; }
		
		[DisplayName("Last Name")]
		public string LastName { get; set; }
		
		[DisplayName("Current Projects")]
		public List<ProjectViewModel> Projects { get; set; }
		
		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }
	}

	public class ConsultantEditViewModel
	{

		[DisplayName("Current Projects")]
		public List<ProjectViewModel> Projects { get; set; }

		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }
		
		[DisplayName("Old Password")]
		[PasswordPropertyText(true)]
		public string OldPassword { get; set; }

		[DisplayName("New Password")]
		[PasswordPropertyText(true)]
		public string NewPassword { get; set; }

		[DisplayName("Confirm Password")]
		[PasswordPropertyText(true)]
		[Compare("NewPassword", ErrorMessage = "Passwords must match")]
		public string ConfirmPassword { get; set; }

		[DisplayName("New Email Address")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string NewEmail { get; set; }

		[DisplayName("Confirm Email Address")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		[Compare("NewEmail", ErrorMessage = "Email addresses must match")]
		public string ConfirmEmail { get; set; }

	}


}