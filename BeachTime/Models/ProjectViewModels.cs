using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{

	/// <summary>
	/// ViewModel for creating a new project.
	/// </summary>
	public class ProjectCreateViewModel
	{
		/// <summary>
		/// Gets or sets the name of the project.
		/// </summary>
		/// <value>
		/// The name of the project.
		/// </value>
		[Required]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is completed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is completed; otherwise, <c>false</c>.
		/// </value>
		[Required]
		[Display(Name = "Completed?")]
		public bool IsCompleted { get; set; }

	}
	/// <summary>
	/// ViewModel for a project.
	/// </summary>
	public class ProjectViewModel
	{
		/// <summary>
		/// Gets or sets the name of the project.
		/// </summary>
		/// <value>
		/// The name of the project.
		/// </value>
		[Required]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is completed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is completed; otherwise, <c>false</c>.
		/// </value>
		[Required]
		[Display(Name = "Completed?")]
		public bool IsCompleted { get; set; }

		/// <summary>
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>
		/// The project identifier.
		/// </value>
		[Required]
		[Display(Name = "Project ID")]		
		public int ProjectId { get; set; }
	}
}