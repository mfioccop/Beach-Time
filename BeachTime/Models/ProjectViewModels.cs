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
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>
		/// The project identifier.
		/// </value>
		[Required]
		[Display(Name = "Project ID")]
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the name of the project.
		/// </summary>
		/// <value>
		/// The name of the project.
		/// </value>
		[Required]
		[Display(Name = "Project Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description of the project.
		/// </value>
		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The project code.
		/// </value>
		[Required]
		[Display(Name = "Project Code")]
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>
		/// The start date.
		/// </value>
		[Required]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date.
		/// </summary>
		/// <value>
		/// The expected end date of the project.
		/// </value>
		[Required]
		[Display(Name = "Projected End Date")]
		public DateTime EndDate { get; set; }

	}
	/// <summary>
	/// ViewModel for a project.
	/// </summary>
	public class ProjectViewModel
	{
		/// <summary>
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>
		/// The project identifier.
		/// </value>
		[Required]
		[Display(Name = "Project ID")]		
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the name of the project.
		/// </summary>
		/// <value>
		/// The name of the project.
		/// </value>
		[Required]
		[Display(Name = "Project Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description of the project.
		/// </value>
		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The project code.
		/// </value>
		[Required]
		[Display(Name = "Project Code")]
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>
		/// The start date.
		/// </value>
		[Required]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date.
		/// </summary>
		/// <value>
		/// The expected end date of the project.
		/// </value>
		[Required]
		[Display(Name = "End Date")]
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets or sets the last updated.
		/// </summary>
		/// <value>
		/// The time when the project was last updated in the database.
		/// </value>
		[Required]
		[Display(Name = "Last Updated")]
		public DateTime LastUpdated { get; set; }

	}
}