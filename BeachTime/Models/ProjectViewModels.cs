using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{

	public class ProjectCreateViewModel
	{
		[Required]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }
		
		[Required]
		[Display(Name = "Completed?")]
		public bool IsCompleted { get; set; }

	}
	public class ProjectViewModel
	{
		[Required]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }
		
		[Required]
		[Display(Name = "Completed?")]
		public bool IsCompleted { get; set; }

		[Required]
		[Display(Name = "Project ID")]		
		public int ProjectId { get; set; }
	}
}