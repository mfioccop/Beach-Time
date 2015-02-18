using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BeachTime.Models
{
	public class ConsultantIndexViewModel : NavbarViewModelBase
	{
		[DisplayName("First Name")]
		public string FirstName { get; set; }
		
		[DisplayName("Last Name")]
		public string LastName { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }
		
		[DisplayName("Projects")]
		public IList<ProjectViewModel> Projects { get; set; }
		
		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }

		[DisplayName("Files")]
		public List<FileIndexViewModel> FileList { get; set; }

		public ConsultantSkillViewModel SkillViewModel { get; set; }

	}

	public class ConsultantEditViewModel : NavbarViewModelBase
	{

		[DisplayName("Projects")]
		public IList<ProjectViewModel> Projects { get; set; }

		public string SkillsString { get; set; }

		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }

		[DisplayName("Files")]
		public List<FileUploadViewModel> FileList { get; set; }

	}

	public class ConsultantSkillViewModel
	{
		[DisplayName("")]
		public string SkillName { get; set; }
	}
}