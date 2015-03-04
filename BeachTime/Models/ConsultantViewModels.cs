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
	/// <summary>
	/// ViewModel for the consultant index page.
	/// </summary>
	public class ConsultantIndexViewModel : NavbarViewModelBase
	{
		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
        [DisplayName("Id")]
        public int Id { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
        [DisplayName("Email")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the projects.
		/// </summary>
		/// <value>
		/// The projects.
		/// </value>
		[DisplayName("Projects")]
		public IList<ProjectViewModel> Projects { get; set; }

		/// <summary>
		/// Gets or sets the skill list.
		/// </summary>
		/// <value>
		/// The skill list.
		/// </value>
		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[DisplayName("Status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the file list.
		/// </summary>
		/// <value>
		/// The file list.
		/// </value>
		[DisplayName("Files")]
		public List<FileIndexViewModel> FileList { get; set; }

		/// <summary>
		/// Gets or sets the skill view model.
		/// </summary>
		/// <value>
		/// The skill view model.
		/// </value>
		public ConsultantSkillViewModel SkillViewModel { get; set; }

	}

	/// <summary>
	/// ViewModel for a consultant's skill.
	/// </summary>
	public class ConsultantSkillViewModel
	{
		/// <summary>
		/// Gets or sets the name of the skill.
		/// </summary>
		/// <value>
		/// The name of the skill.
		/// </value>
		[DisplayName("")]
		public string SkillName { get; set; }
	}
}